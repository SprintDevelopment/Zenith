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
    public class MixtureCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<Mixture>
    {
        public MixtureCreateOrUpdateViewModel(Repository<Mixture> repository, bool containsDeleted = false)
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
                var mixtureItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (mixtureItem is null)
                    PageModel.Items.Add(new MixtureItem { Material = material, MaterialId = material.MaterialId, Percent = 0 });
            });

            RemoveFromItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var mixtureItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (mixtureItem is not null)
                    PageModel.Items.Remove(mixtureItem);
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
