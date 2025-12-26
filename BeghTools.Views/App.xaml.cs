using BeghTools.Core;
using BeghTools.Core.Attributes;
using BeghTools.Core.Interfaces;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using static BeghTools.Core.GlobalHelpers;

namespace BeghTools.Views
{
    /// <summary>
    /// Interaction logic for BeghCore.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            BeghCore.InitializeServices();


            if (startupEventArgs.Args.Length != 0)
                foreach (var type in GetTypesImplementing<IArgumentPlayable>())
                {
                    var attr = type.GetCustomAttribute<ArgumentPlayableAttribute>()!;
                    if (startupEventArgs.Args[0] == attr.ArgumentName)
                    {
                        (GetRequiredService<IArgumentPlayable>(type)).PlayWithArgument(startupEventArgs.Args);
                        Shutdown();
                        return;
                    }

                }
            else
            {
                foreach (var type in GetTypesImplementing<IAutoStartGUI>())
                    GetRequiredService<IAutoStartGUI>(type);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show($"An unhandled exception occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

}
