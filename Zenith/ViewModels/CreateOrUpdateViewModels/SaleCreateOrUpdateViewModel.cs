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
    public class SaleCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<Sale>
    {
        public SaleCreateOrUpdateViewModel(Repository<Sale> repository, bool containsDeleted = false) : base(repository, containsDeleted)
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

            AddToItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var saleItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (saleItem is null)
                    PageModel.Items.Add(new SaleItem { Material = material, MaterialId = material.MaterialId, UnitPrice = 200, Count = 1 });
                else
                    saleItem.Count++;
            });

            RemoveFromItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var saleItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (saleItem is not null)
                    PageModel.Items.Remove(saleItem);
            });

            RemoveAllItemsCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                PageModel.Items.Clear();
            });
        }

        public SourceList<Material> MaterialsSourceList { get; set; } = new SourceList<Material>();
        public ReadOnlyObservableCollection<Material> MaterialsActiveList;

        [Reactive]
        public string SearchedMaterialName { get; set; }

        public ReactiveCommand<Material, Unit> AddToItemsCommand { get; set; }
        public ReactiveCommand<Material, Unit> RemoveFromItemsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveAllItemsCommand { get; set; }
    }
}
