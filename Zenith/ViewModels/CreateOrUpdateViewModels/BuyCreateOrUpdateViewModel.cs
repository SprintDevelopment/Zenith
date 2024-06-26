﻿using DynamicData;
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
        public BuyCreateOrUpdateViewModel(Repository<Buy> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            IObservable<Func<Material, bool>> dynamicFilter = this.WhenAnyValue(vm => vm.SearchedMaterialName)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(materialName => new Func<Material, bool>(m => materialName.IsNullOrWhiteSpace() || m.Name.Contains(materialName)));

            MaterialsSourceList.AddRange(new MaterialRepository().All());

            MaterialsSourceList.Connect()
                .Filter(dynamicFilter, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(out MaterialsActiveList)
                .Subscribe();

            AddToItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var buyItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (buyItem is null)
                    PageModel.Items.Add(new BuyItem { Material = material, MaterialId = material.MaterialId, UnitPrice = material.BuyPrice, BuyCountUnit = material.CommonBuyUnit, Count = 1 });
                else
                    buyItem.Count++;
            });

            RemoveFromItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var buyItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (buyItem is not null)
                    PageModel.Items.Remove(buyItem);
            });

            RemoveAllItemsCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                PageModel.Items.Clear();
            });
        }

        public SourceList<Material> MaterialsSourceList { get; set; } = new SourceList<Material>();
        public ReadOnlyObservableCollection<Material> MaterialsActiveList;

        [Reactive]
        public bool IsCountSelectorVisible { get; set; }

        [Reactive]
        public string SearchedMaterialName { get; set; }

        public ReactiveCommand<Material, Unit> AddToItemsCommand { get; set; }
        public ReactiveCommand<Material, Unit> RemoveFromItemsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveAllItemsCommand { get; set; }
    }
}
