using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chess
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Window _mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainVM = new MainWindowVM();
            _mainWindow = new MainWindow { DataContext = mainVM };
            _mainWindow.Show();
            _mainWindow.Focus();
        }
    }
}
