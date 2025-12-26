using BeghTools.Core.Interfaces;
using BeghTools.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;
namespace BeghToolsUi.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , ISingletonable , IAutoStartGUI
    {
        MainWindowViewModel ViewModel;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
            Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Method to handle dragging the window
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
