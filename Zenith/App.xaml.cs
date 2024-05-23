using DynamicData;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Data;
using Zenith.ViewModels;

namespace Zenith
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel MainViewModel { get; set; } = new MainViewModel();
        public static ApplicationDbContext Context { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Exit += (s, e) => { if (WordUtil.wordApp != null && !WordUtil.wordApp.Visible) WordUtil.wordApp.Quit(false); };
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(@"Logs\ZLog-.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}")
                    .CreateLogger();

            Log.Information("Application started.");

            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(exp =>
            {
                MainViewModel._alerts.Add(new AlertViewModel
                {
                    Guid = new Guid(),
                    Title = "An error occurred",
                    Description = exp.Message,
                    DialogType = DialogTypes.Danger,
                    ActionContent = "Got it",
                    ActionCommand = ReactiveCommand.Create<Unit>(_ => { })
                });
            });

            DispatcherUnhandledException += (s, ee) =>
            {
                ee.Handled = true;
            };
        }
    }
}
