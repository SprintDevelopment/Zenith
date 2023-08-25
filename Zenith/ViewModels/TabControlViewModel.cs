using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using DynamicData;
using Zenith.Assets.UI.UserControls;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace Zenith.ViewModels
{
    public class TabControlViewModel : ReactiveObject
    {
        public TabControlViewModel()
        {
            _tabs.Connect()
                .Bind(out Tabs)
                .Subscribe();

            _tabs.Connect()
                .MergeMany(t => t.WhenAnyValue(x => x.IsSelected).Where(i => i).Select(_ => SelectedTabViewModel = t))
                .Subscribe();

            _tabs.Connect()
                .MergeMany(t => t.CloseCommand.Select(_ => t))
                .Do(_ => { var tabToSelect = _tabs.Items.OrderBy(tvm => tvm.SelectionOrder).Skip(1).FirstOrDefault(); if (tabToSelect is not null) tabToSelect.IsSelected = true; })
                .Delay(TimeSpan.FromMilliseconds(100))
                .Do(tabToRemove => _tabs.Remove(tabToRemove))
                .Subscribe();

            this.WhenAnyValue(vm => vm.SelectedTabViewModel)
                .WhereNotNull()
                .SelectMany(stvm => _tabs.Items.Select(tab => { tab.IsSelected = tab == stvm; tab.SelectionOrder = tab == stvm ? 0 : ++tab.SelectionOrder; return tab; }))
                .Subscribe();
        }

        public SourceList<TabViewModel> _tabs { get; private set; } = new SourceList<TabViewModel>();
        public ReadOnlyObservableCollection<TabViewModel> Tabs;

        [Reactive]
        public TabViewModel SelectedTabViewModel { get; set; }
    }
}
