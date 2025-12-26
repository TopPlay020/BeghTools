namespace BeghTools.ViewModels.Shared
{
    public partial class ContextMenuItemModel : ObservableObject
    {
        [ObservableProperty]
        public string description = default!;
        [ObservableProperty]
        public string iconPath = default!;
        public IContextMenuAddable ContextMenuAddable;
        [ObservableProperty]
        public bool isInstalled;
        [ObservableProperty]
        public bool isInstalling;
    }
}
