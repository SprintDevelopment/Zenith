using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        public SettingsViewModel()
        {
                
        }

        [Reactive]
        public BackupIntervals BackupInterval { get; set; }

        [Reactive]
        public string BackupDefaultLocation { get; set; } = "";

        [Reactive]
        public string BackupEmail { get; set; } = "";

        public ReactiveCommand<Unit, Unit> GetSettings { get; private set; }
        public ReactiveCommand<Unit, Unit> SaveSettings { get; private set; }
    }
}
