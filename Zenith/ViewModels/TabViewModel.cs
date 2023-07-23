using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace Zenith.ViewModels
{
    public class TabViewModel : ReactiveObject
    {
        public TabViewModel()
        {
            SelectCommand = ReactiveCommand.Create(() => { IsSelected = true; });
            CloseCommand = ReactiveCommand.Create<Unit, Guid>(_ => Guid);
        }

        [Reactive]
        public Guid Guid { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public bool IsSelected { get; set; }

        [Reactive]
        public bool AllowClose { get; set; }

        public ReactiveCommand<Unit, Unit> SelectCommand { get; set; }
        public ReactiveCommand<Unit, Guid> CloseCommand { get; set; }
    }
}
