using System.Drawing;

namespace BeghTools.ViewModels.Pages
{
    [PageInfo(false, "Image Converter", "/Assets/Icons/ImageEditor.png")]
    public partial class ImageConverterViewModel : ObservableObject, IPageMenu, ITransientable
    {
        private const string DefaultImagePath = "/Assets/Icons/Image.png";
        [ObservableProperty]
        private List<string> imageTypes;
        [ObservableProperty]
        private string outputImageType;
        [ObservableProperty]
        private string sourceImagePath = DefaultImagePath;
        [ObservableProperty]
        private string? unsupportedImageInput = null;
        [ObservableProperty]
        private string outputImagePath = DefaultImagePath;
        [ObservableProperty]
        private string? unsupportedImageOutput = null;
        [ObservableProperty]
        private string selectedColorHex = "#FFFFFF";
        [ObservableProperty]
        private int outputWidth = 100;
        [ObservableProperty]
        private int outputHeight = 100;
        [ObservableProperty]
        private bool maintainAspectRatio = true;

        [ObservableProperty]
        private bool canConvert = false;
        [ObservableProperty]
        private bool isConverting = false;
        [ObservableProperty]
        private bool isJPEG = false;
        [ObservableProperty]
        private bool readyToExport = false;

        public bool IsConvertMenuEnabled => CanConvert && !IsConverting;

        partial void OnCanConvertChanged(bool oldValue, bool newValue) { OnPropertyChanged(nameof(IsConvertMenuEnabled)); }
        partial void OnIsConvertingChanged(bool oldValue, bool newValue) { OnPropertyChanged(nameof(IsConvertMenuEnabled)); }

        private ImageConversionService _imageConversionService;
        private IUserDialogService _userDialogService;
        public ImageConverterViewModel(ImageConversionService imageConversionService, IUserDialogService userDialogService)
        {
            imageTypes = ImageConversionService.SupportedFormats;
            outputImageType = imageTypes[0];
            _imageConversionService = imageConversionService;
            _userDialogService = userDialogService;
        }

        partial void OnOutputImageTypeChanged(string value)
        {
            IsJPEG = value == "jpg";
            ReadyToExport = false;
        }

        private void SelectFilePath(string SelectedFilePath)
        {
            SourceImagePath = SelectedFilePath;
            ReadyToExport = false;
            CanConvert = true;
            OutputImagePath = DefaultImagePath;
        }

        public void DropSourceImagePathCommand(string SelectedFilePath)
        {
            var ext = Path.GetExtension(SelectedFilePath).TrimStart('.');
            if (ImageTypes.Contains(ext.ToLower()))
                SelectFilePath(SelectedFilePath);
        }

        [RelayCommand]
        private void SelectSourceImagePath()
        {
            var OldCanConvert = CanConvert;
            CanConvert = false;
            var selectedFilePath = _userDialogService.OpenFile(
                  "Select Source Image",
                  "Image Files|" + string.Join(";", ImageTypes.Select(f => "*." + f))
                );
            if (selectedFilePath == null)
                CanConvert = OldCanConvert;
            else
            {
                var selectedFileExtension = Path.GetExtension(selectedFilePath).ToLower().TrimStart('.');
                if (WPFImageUnsupportedFormats.Contains(selectedFileExtension))
                    UnsupportedImageInput = selectedFileExtension;
                else
                    UnsupportedImageInput = null;
                SelectFilePath(selectedFilePath);
            }
        }

        [RelayCommand]
        private void SelectColor()
        {

        }

        [RelayCommand]
        private async Task ConvertImage()
        {
            string TempDir = GetRequiredService<TempService>().TempDir;
            string tempFile = Path.Combine(TempDir, $"{Guid.NewGuid()}.{OutputImageType}");
            IsConverting = true;
            CanConvert = false;
            ReadyToExport = false;

            Color color = ColorTranslator.FromHtml(SelectedColorHex);
            _imageConversionService.ConvertImage(SourceImagePath, tempFile, OutputImageType, color);

            IsConverting = false;
            CanConvert = true;
            ReadyToExport = true;
            if (WPFImageUnsupportedFormats.Contains(OutputImageType))
                UnsupportedImageOutput = OutputImageType;
            else
                UnsupportedImageOutput = null;
            OutputImagePath = tempFile;
        }

        [RelayCommand]
        private void ExportImage()
        {
            var suggestedName = $"{Path.GetFileNameWithoutExtension(SourceImagePath)}.{OutputImageType!}";
            var fileName = _userDialogService.SaveFile(
                "Export Converted Image",
                suggestedName,
                $"{OutputImageType.ToUpper()} Files|*.{OutputImageType.ToLower()}"
                );
            if (fileName != null)
            {
                File.Copy(OutputImagePath, fileName, true);
            }
        }
    }
}