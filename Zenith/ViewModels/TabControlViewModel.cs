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
                .MergeMany(t => t.WhenAnyValue(x => x.Guid, x => x.IsSelected))
                .Select(change => new { guid = change.Item1, isSelected = change.Item2 })
                .Where(x => x.isSelected)
                .Select(selectedItem => _tabs.Items.FirstOrDefault(t => t.IsSelected && t.Guid != selectedItem.guid))
                .WhereNotNull()
                .Do(prevSelectedItem => prevSelectedItem.IsSelected = false)
                .Subscribe();

            _tabs.Connect()
                .MergeMany(t => t.CloseCommand)
                .Select(guid => _tabs.Items.FirstOrDefault(t => t.Guid == guid))
                .Delay(TimeSpan.FromMilliseconds(100))
                .Do(tabToRemove => _tabs.Remove(tabToRemove))
                .Subscribe();
        }

        public SourceList<TabViewModel> _tabs { get; private set; } = new SourceList<TabViewModel>();
        public ReadOnlyObservableCollection<TabViewModel> Tabs;

        [Reactive]
        public TabViewModel SelectTabViewModel { get; set; }
    }
}
