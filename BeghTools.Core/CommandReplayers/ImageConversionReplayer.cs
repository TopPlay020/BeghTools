using BeghTools.Core.Attributes;
using BeghTools.Core.Interfaces;
using BeghTools.Core.Services;
using static BeghTools.Core.GlobalHelpers;
using Microsoft.Win32;

namespace BeghTools.Core.CommandReplays
{
    [ArgumentPlayable("ImageConversion", "Add Context Menu Making converting Image Type easier.", "/Assets/Icons/ImageEditor.png")]

    internal class ImageConversionReplayer : IArgumentPlayable, ITransientable, IContextMenuAddable
    {
        private ImageConversionService _imageConversionService;
        private IUserDialogService _userDialogService;
        public ImageConversionReplayer(ImageConversionService imageConversionService, IUserDialogService userDialogService)
        {
            _imageConversionService = imageConversionService;
            _userDialogService = userDialogService;
        }

        public void PlayWithArgument(string[] Args)
        {
            var inputImagePath = Args[1];
            var outputImageType = Args[2];

            if (File.Exists(inputImagePath))
            {
                var outputImagePath = Path.ChangeExtension(inputImagePath, outputImageType);
                if (File.Exists(outputImagePath))
                    _userDialogService.ShowMessage($"Output image file already exists: {outputImagePath}", "Error");
                else
                    _imageConversionService.ConvertImage(inputImagePath, outputImagePath, outputImageType);
            }
            else
            {
                _userDialogService.ShowMessage($"Input image file does not exist: {inputImagePath}", "Error");
            }

        }

        public void AddToContextMenu()
        {
            var supportedTypes = ImageConversionService.SupportedFormats;
            string execPath = GetCurrentExecutablePath();
            using var baseKey = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations", true);

            foreach (var ext in supportedTypes)
            {
                string mainKeyPath = $@".{ext}\shell\ConvertImageType";
                using var mainKey = baseKey.CreateSubKey(mainKeyPath);
                mainKey.SetValue("MUIVerb", "Convert Image Type");
                mainKey.SetValue("Icon", execPath);
                mainKey.SetValue("SubCommands", ""); // Empty string enables submenu!

                // Create shell subfolder for subcommands
                string shellPath = $@"{mainKeyPath}\shell";
                using var shellKey = baseKey.CreateSubKey(shellPath);

                // Create each conversion option
                foreach (var type in supportedTypes)
                {
                    if (type.Equals(ext)) continue;

                    string subKeyName = $"ConvertTo{type.ToUpper()}";

                    using var subKey = shellKey.CreateSubKey(subKeyName);
                    subKey.SetValue("", $"Convert to {type.ToUpper()}");
                    subKey.SetValue("Icon", execPath);

                    using var cmdKey = subKey.CreateSubKey("command");
                    cmdKey.SetValue("", $"\"{execPath}\" ImageConversion \"%1\" {type}");
                }
            }
        }

        public bool ExistsInContextMenu()
        {
            using var key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\.png\shell\ConvertImageType");
            return key != null;
        }

        public void RemoveFromContextMenu()
        {
            string[] types = { ".png", ".jpg", ".jpeg", ".bmp", ".ico", ".gif" };
            using var baseKey = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations", true);

            foreach (var ext in types)
            {
                try
                {
                    baseKey.DeleteSubKeyTree($@"{ext}\shell\ConvertImageType", false);
                }
                catch { }
            }
        }


    }
}
