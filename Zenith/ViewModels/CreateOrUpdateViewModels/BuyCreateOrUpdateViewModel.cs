using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using System.Reactive;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class BuyCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<Buy>
    {
        public BuyCreateOrUpdateViewModel(Repository<Buy> repository, bool containsDeleted = false) : base(repository, containsDeleted)
        {
            IObservable<Func<Material, bool>> dynamicFilter = this.WhenAnyValue(vm => vm.SearchedMaterialName)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(SynchronizationContext.Current)
                .Select(materialName => new Func<Material, bool>(m => materialName.IsNullOrWhiteSpace() || m.Name.Contains(materialName)));

            MaterialsSourceList.AddRange(new MaterialRepository().All());

            MaterialsSourceList.Connect()
                .Filter(dynamicFilter, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(out MaterialsActiveList)
                .Subscribe();

            BuyItemsSourceList.Connect()
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(out BuyItemsActiveList)
                .Subscribe();

            AddToItemsCommand = ReactiveCommand.Create<Material>(item =>
            {
                var buyItem = BuyItemsSourceList.Items.FirstOrDefault(b => b.Material == item);
                if (buyItem is null)
                    BuyItemsSourceList.Add(new BuyItem { Material = item, UnitPrice = 200, Count = 1 });
                else
                    buyItem.Count++;
            });

            RemoveFromItemsCommand = ReactiveCommand.Create<BuyItem>(item =>
            {
                BuyItemsSourceList.Remove(item);
            });

            RemoveAllItemsCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                BuyItemsSourceList.Clear();
            });
        }

        public SourceList<Material> MaterialsSourceList { get; set; } = new SourceList<Material>();
        public ReadOnlyObservableCollection<Material> MaterialsActiveList;

        public SourceList<BuyItem> BuyItemsSourceList { get; set; } = new SourceList<BuyItem>();
        public ReadOnlyObservableCollection<BuyItem> BuyItemsActiveList;

        [Reactive]
        public string SearchedMaterialName { get; set; }

        public ReactiveCommand<Material, Unit> AddToItemsCommand { get; set; }
        public ReactiveCommand<BuyItem, Unit> RemoveFromItemsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveAllItemsCommand { get; set; }
    }
}
