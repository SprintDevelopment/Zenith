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
using Zenith.Assets.Utils;
using Zenith.Views.CreateOrUpdateViews;
using System.Diagnostics;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class SaleCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<Sale>
    {
        public SaleCreateOrUpdateViewModel(Repository<Sale> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            IObservable<Func<Material, bool>> dynamicFilter = this.WhenAnyValue(vm => vm.SearchedMaterialName)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(materialName => new Func<Material, bool>(m => materialName.IsNullOrWhiteSpace() || m.Name.Contains(materialName)));

            MaterialsSourceList.AddRange(new MaterialRepository().AllIncludeMixed());

            MaterialsSourceList.Connect()
                .Filter(dynamicFilter, ListFilterPolicy.ClearAndReplace)
                .Transform((item, i) => { item.DisplayOrder = i + 1; return item; })
                .Bind(out MaterialsActiveList)
                .Subscribe();

            AddToItemsCommand = ReactiveCommand.Create<Material>(material =>
            {
                var saleItem = PageModel.Items.FirstOrDefault(b => b.Material.MaterialId == material.MaterialId);
                if (saleItem is null)
                    PageModel.Items.Add(new SaleItem { Material = material, MaterialId = material.MaterialId, UnitPrice = material.SalePrice, SaleCountUnit = material.CommonSaleUnit, Count = 1 });
                else
                    saleItem.Count++;
            });

            RemoveFromItemsCommand = ReactiveCommand.Create<SaleItem>(saleItem =>
            {
                PageModel.Items.Remove(saleItem);
            });

            RemoveAllItemsCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                PageModel.Items.Clear();
            });

            ShowHideDeliveriesCommand = ReactiveCommand.Create<SaleItem>(saleItem =>
            {
                saleItem.IsDeliveriesVisible = !saleItem.IsDeliveriesVisible;
            });

            var deliveryCreateOrUpdatePage = new DeliveryPage();

            IDisposable createUpdateDisposable = null;
            AddNewDeliveryCommand = ReactiveCommand.Create<SaleItem>(saleItem =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = deliveryCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        saleItem.Deliveries.AddRange(changeSet);
                    }
                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                deliveryCreateOrUpdatePage.ViewModel.PrepareCommand.Execute().Subscribe();

                //Important
                deliveryCreateOrUpdatePage.ViewModel.PageModel.SaleItem = saleItem;
                deliveryCreateOrUpdatePage.ViewModel.PageModel.SaleItemId = saleItem.SaleItemId;
                deliveryCreateOrUpdatePage.ViewModel.PageModel.Count = saleItem.Count;
                deliveryCreateOrUpdatePage.ViewModel.PageModel.CashState = PageModel.CashState;
                deliveryCreateOrUpdatePage.ViewModel.PageModel.IsIndirectDelivery = PageModel.IsIndirectSale;
                deliveryCreateOrUpdatePage.ViewModel.PageModel.DateTime = deliveryCreateOrUpdatePage.ViewModel.PageModel.SourceDeliveryDateTime = PageModel.DateTime;


                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(deliveryCreateOrUpdatePage).Subscribe();
            });

            UpdateDeliveryCommand = ReactiveCommand.Create<Delivery>(deliveryToUpdate =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = deliveryCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        MapperUtil.Mapper.Map(changeSet.FirstOrDefault(), deliveryToUpdate);
                    }

                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                deliveryCreateOrUpdatePage.ViewModel.PrepareCommand.Execute(deliveryToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(deliveryCreateOrUpdatePage).Subscribe();
            });

            this.WhenAnyValue(vm => vm.PageModel).Where(pm => pm != null).Subscribe(pm =>
            {
                CreateOrUpdateAndContinueCommand = ReactiveCommand.Create<Unit>(_ =>
                {
                    var cuCommandResult = IsNew ? Repository.Add(PageModel) : Repository.Update(PageModel, PageModel.GetKeyPropertyValue());
                    PageModel = Repository.Single(PageModel.GetKeyPropertyValue());
                    
                    ChangeSet.Clear();
                    ChangeSet.Add(PageModel);
                }, PageModel.ValidationContext.WhenAnyValue(context => context.IsValid));

                CreateOrUpdateAndReturnCommand = ReactiveCommand.Create<Unit>(_ =>
                {
                    var cuCommandResult = IsNew ? Repository.Add(PageModel) : Repository.Update(PageModel, PageModel.GetKeyPropertyValue());
                    
                    ChangeSet.Clear();
                    ChangeSet.Add(Repository.Single(PageModel.GetKeyPropertyValue()));

                    ReturnCommand.Execute().Subscribe().Dispose();
                }, PageModel.ValidationContext.WhenAnyValue(context => context.IsValid));
            });
        }

        public SourceList<Material> MaterialsSourceList { get; set; } = new SourceList<Material>();
        public ReadOnlyObservableCollection<Material> MaterialsActiveList;

        [Reactive]
        public bool IsCountSelectorVisible { get; set; }

        [Reactive]
        public string SearchedMaterialName { get; set; }
        public ReactiveCommand<Unit, Unit> CreateOrUpdateAndContinueCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateOrUpdateAndReturnCommand { get; set; }
        public ReactiveCommand<Material, Unit> AddToItemsCommand { get; set; }
        public ReactiveCommand<SaleItem, Unit> RemoveFromItemsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveAllItemsCommand { get; set; }
        public ReactiveCommand<SaleItem, Unit> ShowHideDeliveriesCommand { get; set; }
        public ReactiveCommand<SaleItem, Unit> AddNewDeliveryCommand { get; set; }
        public ReactiveCommand<Delivery, Unit> UpdateDeliveryCommand { get; set; }
    }
}
