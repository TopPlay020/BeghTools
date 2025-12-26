namespace BeghTools.ViewModels.Shared
{
    public partial class NetworkInterfaceViewModel : ObservableObject
    {
        public string Id;
        [ObservableProperty]
        public string name = default!;
        [ObservableProperty]
        public bool isUp = default!;
    }
}
