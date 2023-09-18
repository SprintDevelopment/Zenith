using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Zenith.Assets.Utils;
using Zenith.ViewModels;

namespace Zenith
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel MainViewModel { get; set; } = new MainViewModel();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Exit += (s, e) => { if (WordUtil.wordApp != null && !WordUtil.wordApp.Visible) WordUtil.wordApp.Quit(false); };

            DispatcherUnhandledException += (s, ee) => { MessageBox.Show(ee.Exception.Message); ee.Handled = true; };
        }
    }
}
