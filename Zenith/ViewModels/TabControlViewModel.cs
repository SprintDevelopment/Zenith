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
                .Do(stvm => stvm.SelectionOrder = 0)
                .SelectMany(stvm => _tabs.Items.Where(tab => tab != stvm).Select(tab => { tab.IsSelected = false; tab.AllowClose = true; tab.SelectionOrder++; return tab; }))
                .Subscribe();

            _tabs.CountChanged
                .Where(i => i == 1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(_ => _tabs.Items.Single().AllowClose = false)
                .Subscribe();
        }

        public SourceList<TabViewModel> _tabs { get; private set; } = new SourceList<TabViewModel>();
        public ReadOnlyObservableCollection<TabViewModel> Tabs;

        [Reactive]
        public TabViewModel SelectedTabViewModel { get; set; }
    }
}
