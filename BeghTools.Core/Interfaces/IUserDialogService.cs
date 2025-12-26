namespace BeghTools.Core.Interfaces
{
    public interface IUserDialogService
    {
        void ShowMessage(string message, string caption = "");

        // OpenFileDialog
        string OpenFile(string title, string filter);

        // SaveFileDialog
        string SaveFile(string title, string defaultFileName, string filter);
    }
}
