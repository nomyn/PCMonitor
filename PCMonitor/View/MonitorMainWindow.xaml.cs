using System.Reflection;
using System.Windows;

namespace PCMonitor.View
{
    /// <summary>
    /// Interaction logic for MonitorMainWindow.xaml
    /// </summary>
    public partial class MonitorMainWindow : Window
    {
        public MonitorMainWindow()
        {
            InitializeComponent();            

            // set title with version number
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            MainWindow.Title = $"PC monitor {version}";
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
