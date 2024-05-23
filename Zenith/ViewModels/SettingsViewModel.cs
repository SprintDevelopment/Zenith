using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        public SettingsViewModel()
        {
            var configurationRepository = new ConfigurationRepository();

            var allConfigs = configurationRepository.All().AsEnumerable();

            if (Enum.TryParse<BackupIntervals>(allConfigs.SingleOrDefault(c => c.Key == nameof(BackupInterval))?.Value ?? $"{BackupIntervals.DontCare}", out BackupIntervals backupInterval))
                BackupInterval = backupInterval;
            BackupDefaultLocation = allConfigs.SingleOrDefault(c => c.Key == nameof(BackupDefaultLocation))?.Value ?? "";
            BackupEmail = allConfigs.SingleOrDefault(c => c.Key == nameof(BackupEmail))?.Value ?? "";

            SaveSettingsCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                configurationRepository.AddOrUpdateRange(new List<Configuration>
                {
                    new Configuration { Key = nameof(BackupInterval), Value = $"{BackupInterval}"},
                    new Configuration { Key = nameof(BackupDefaultLocation), Value = BackupDefaultLocation},
                    new Configuration { Key = nameof(BackupEmail), Value = BackupEmail}
                });
            });

            CloseCommand = ReactiveCommand.CreateFromObservable<Unit>(() => App.MainViewModel.CreateUpdatePageReturnedCommand.Execute());
        }

        [Reactive]
        public bool IsBackupSettingsSectionVisible { get; set; } = true;

        [Reactive]
        public bool IsReminderSettingsSectionVisible { get; set; }

        [Reactive]
        public BackupIntervals BackupInterval { get; set; }

        [Reactive]
        public string BackupDefaultLocation { get; set; } = "";

        [Reactive]
        public string BackupEmail { get; set; } = "";

        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    }
}
