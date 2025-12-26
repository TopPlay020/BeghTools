using BeghTools.Core.Interfaces;

namespace BeghTools.Views.Services
{
    public class UserDialogService : IUserDialogService , ISingletonable , IRegisterByInterface<IUserDialogService>
    {
        public void ShowMessage(string message, string caption = "")
        {
            System.Windows.MessageBox.Show(message, caption);
        }

        public string OpenFile(string title, string filter)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = title,
                Filter = filter
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string SaveFile(string title, string defaultFileName, string filter)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = title,
                FileName = defaultFileName,
                Filter = filter
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
