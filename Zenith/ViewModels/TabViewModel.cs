using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace Zenith.ViewModels
{
    public class TabViewModel : ReactiveObject
    {
        public TabViewModel()
        {
            SelectCommand = ReactiveCommand.Create(() => { IsSelected = true; });
            CloseCommand = ReactiveCommand.Create<Unit, TabViewModel>(_ => this);
        }

        [Reactive]
        public bool IsSelected { get; set; }

        [Reactive]
        public bool AllowClose { get; set; }

        [Reactive]
        public int SelectionOrder { get; set; }

        [Reactive]
        public Page RelatedMainPage { get; set; }

        [Reactive]
        public Page RelatedCreateUpdatePage { get; set; }

        public ReactiveCommand<Unit, Unit> SelectCommand { get; set; }
        public ReactiveCommand<Unit, TabViewModel> CloseCommand { get; set; }
    }
}
