using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace BeghTools.ViewModels.Pages
{
    [PageInfo(false, "Monitoring", "/Assets/Icons/Monitoring.png")]
    public partial class MonitoringViewModel : ObservableObject, IPageMenu, ITransientable
    {
        private MonitoringService _monitoringService;
        [ObservableProperty]
        private List<NetworkInterfaceViewModel> networkInterface;
        [ObservableProperty]
        private NetworkInterfaceViewModel selectedNetworkInterface;

        [ObservableProperty]
        private string downloadSpeed = "0 kb/s";
        [ObservableProperty]
        private string uploadSpeed = "0 kb/s";

        [ObservableProperty]
        public Axis[] xAxes, yAxes;
        [ObservableProperty]
        public ISeries[] series;
        [ObservableProperty]
        public SolidColorPaint legendTextPaint = new SolidColorPaint { Color = SKColors.White };
        private ObservableCollection<ObservableValue> DownloadSpeedHistoryList;
        private ObservableCollection<ObservableValue> UploadSpeedHistoryList;
        [ObservableProperty]
        public TimeSpan animationSpeed = TimeSpan.FromSeconds(1);
        [ObservableProperty]
        public Func<float, float> easingFunction = EasingFunctions.Lineal;

        public MonitoringViewModel(MonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
            NetworkInterface = _monitoringService.GetCurrentNetworkInterfaces().Select(nis => new NetworkInterfaceViewModel
            {
                Id = nis.Id,
                Name = nis.Name,
                IsUp = nis.IsUp
            }).ToList();
            SelectedNetworkInterface = NetworkInterface.FirstOrDefault(ni => ni.IsUp == true);

            InitGraph();

            _monitoringService.OnNetworkStatusChanged += OnNetworkInterfaceStatusChanged;
            _monitoringService.OnBandwidthChanged += OnBandwidthChanged;
        }

        public void InitGraph()
        {

            XAxes = new[]
            {
                new Axis
                {
                    IsVisible = false,
                    MinLimit = 0, MaxLimit = 60,
                }
            };

            YAxes = [
                new Axis
                {
                    Labeler = value =>
                    {
                        if (value >= 1024)
                            return $"{value / 1024.0:0.##} MB";
                        else
                            return $"{value:0.##} KB";
                    },
                    ShowSeparatorLines = false,
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    MinLimit = 0
                }
            ];

            DownloadSpeedHistoryList = new() { new ObservableValue(0) };
            UploadSpeedHistoryList = new() { new ObservableValue(0) };
            Series = [
                new LineSeries<ObservableValue>
                    {
                        IsHoverable = false,
                        Name = "Downlaod",
                        GeometrySize = 0,
                        Values = DownloadSpeedHistoryList,
                    },
                    new LineSeries<ObservableValue>
                    {
                        IsHoverable = false,
                        Name = "Upload",
                        GeometrySize = 0,
                        Values = UploadSpeedHistoryList,
                    }
                ];

        }
        public void OnUnload()
        {
            _monitoringService.OnNetworkStatusChanged -= OnNetworkInterfaceStatusChanged;
            _monitoringService.OnBandwidthChanged -= OnBandwidthChanged;

        }
        partial void OnSelectedNetworkInterfaceChanged(NetworkInterfaceViewModel? value)
        {
            DownloadSpeedHistoryList?.Clear();
            UploadSpeedHistoryList?.Clear();
            OnBandwidthChanged(0, 0);
            _monitoringService.SetMonitoredInterface(value?.Id);
        }
        public void OnNetworkInterfaceStatusChanged(string id, bool isUp)
        {
            var ni = NetworkInterface.FirstOrDefault(n => n.Id == id);
            if (ni == null) return;
            ni.IsUp = isUp;
            if (ni != SelectedNetworkInterface && SelectedNetworkInterface != null) return;
            SelectedNetworkInterface = NetworkInterface.FirstOrDefault(ni => ni.IsUp == true);
        }

        public void OnBandwidthChanged(int downloadKbps, int uploadKbps)
        {
           
            DownloadSpeedHistoryList?.Add(new ObservableValue(downloadKbps));
            UploadSpeedHistoryList?.Add(new ObservableValue(uploadKbps));
            if (DownloadSpeedHistoryList?.Count > 60)
            {
                DownloadSpeedHistoryList?.RemoveAt(0);
                UploadSpeedHistoryList?.RemoveAt(0);
            }
            string FormatSpeed(int kbps)
            {
                if (kbps >= 1024)
                    return $"{kbps / 1024.0:F2} mb/s";
                else
                    return $"{kbps} kb/s";
            }
            DownloadSpeed = FormatSpeed(downloadKbps);
            UploadSpeed = FormatSpeed(uploadKbps);
        }
    }
}
