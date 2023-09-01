using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Assets.Extensions;
using System.Collections.ObjectModel;
using DynamicData.Binding;

namespace Zenith.ViewModels.ListViewModels
{
    public class DeliveryListViewModel : BaseListViewModel<Delivery>
    {
        public DeliveryListViewModel(DeliveryRepository repository, SearchBaseDto searchModel, IObservable<Func<Delivery, bool>> criteria, Sale relatedSale)
            : base(repository, searchModel, criteria, PermissionTypes.Sales)
        {
            var modelAttributes = typeof(Delivery).GetAttribute<ModelAttribute>();
            ViewTitle = $"لیست {modelAttributes.MultipleName} به {relatedSale.Company.Name} بابت سفارش شماره {relatedSale.SaleId}";

            SourceList.Clear();
            SourceList.AddRange(Repository.Find(d => relatedSale.Items.Contains(d.SaleItem)));


            SourceList.Connect()
                .Filter(criteria, ListFilterPolicy.ClearAndReplace)
                .Sort(SortExpressionComparer<Delivery>.Ascending(d => d.DateTime))
                .GroupOn(d => d.SaleItem)
                .Transform(g => 
                {
                    var saleItem = g.GroupKey;
                    saleItem.Deliveries = g.List.Items.ToObservableCollection();

                    return saleItem;
                })
                .Bind(out SaleItemsActiveList)
                .Subscribe();

            ShowHideDeliveriesCommand = ReactiveCommand.Create<SaleItem>(saleItem =>
            {
                saleItem.IsDeliveriesVisible = !saleItem.IsDeliveriesVisible;
            });

            AddNewDeliveryCommand = ReactiveCommand.CreateFromObservable<SaleItem, Unit>(saleItem => 
                CreateCommand.Execute()
                .Do(_ => CreateUpdatePage.ViewModel.PageModel = new Delivery { SaleItem = saleItem, SaleItemId = saleItem.SaleItemId }));

        }

        public ReadOnlyObservableCollection<SaleItem> SaleItemsActiveList;
        public ReactiveCommand<SaleItem, Unit> ShowHideDeliveriesCommand { get; set; }
        public ReactiveCommand<SaleItem, Unit> AddNewDeliveryCommand { get; set; }
    }
}
