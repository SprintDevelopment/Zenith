using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;
using System.Reactive.Linq;

namespace Zenith.ViewModels
{
    public class AlertViewModel : ReactiveObject
    {
        public AlertViewModel()
        {
            CloseCommand = ReactiveCommand.Create<Unit, Guid>(_ => Guid);
        }

        [Reactive]
        public Guid Guid { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public string Description { get; set; }

        [Reactive]
        public string ActionContent { get; set; }

        [Reactive]
        public DialogTypes DialogType { get; set; }

        public ReactiveCommand<Unit, Guid> CloseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ActionCommand { get; set; }
    }
}
