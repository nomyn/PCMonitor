using System.Windows;
using PCMonitor.Helpers;

namespace PCMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            FileLogger.Log(e.Exception.Message);
            FileLogger.Log(e.Exception.InnerException.Message);
            FileLogger.Log(e.Exception.StackTrace);
        }
    }
}
