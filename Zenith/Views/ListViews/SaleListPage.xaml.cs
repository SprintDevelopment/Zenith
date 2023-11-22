using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.SearchModels;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for SaleListPage.xaml
    /// </summary>
    public partial class SaleListPage : BaseListPage<Sale>
    {
        public SaleListPage()
        {
            InitializeComponent();
            var searchModel = new SaleSearchModel();

            IObservable<Func<Sale, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Sale, bool>(sale =>
                    (s.CompanyId == 0 || sale.CompanyId == s.CompanyId) &&
                    (s.DateRange == DateRanges.DontCare || sale.DateTime.IsInDateRange(s.DateRange))));

            ViewModel = new SaleListViewModel(new SaleRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new SalePage()
            };

            var CastedViewModel = (SaleListViewModel)ViewModel;

            this.WhenActivated(d =>
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                CastedViewModel.WhenAnyValue(vm => vm.SalesPrePrintDto.SearchedSiteName, vm => vm.SalesPrePrintDto.SearchedMaterialName, vm => vm.SalesPrePrintDto.LpoNumber)
                    .Select(x => new { searchedSiteName = x.Item1, searchedMaterialName  = x.Item2, lpoNumber = x.Item3})
                    .Do(x =>
                    {
                        siteSearchHintTextBlock.Visibility = x.searchedSiteName.IsNullOrWhiteSpace().Viz();
                        materialSearchHintTextBlock.Visibility = x.searchedMaterialName.IsNullOrWhiteSpace().Viz();
                        lpoNumberHintTextBlock.Visibility = x.lpoNumber.IsNullOrWhiteSpace().Viz();
                    })
                    .Subscribe().DisposeWith(d);

                var modalBackRect = ((Grid)Content).Children.OfType<Rectangle>().Single(r => r.Name == "modalBackRect");
                modalBackRect.InputBindings.Add(new MouseBinding(CastedViewModel.HidePrePrintGridCommand, new MouseGesture(MouseAction.LeftClick)));

                CastedViewModel.SalesPrePrintDto.WhenAnyValue(dto => dto.FilteredByLpo)
                    .Select(fbl => fbl ? new SolidColorBrush(Color.FromArgb(255, 205, 0, 0)) : new SolidColorBrush(Color.FromArgb(255, 203, 203, 203)))
                    .Do(color =>
                    {
                        lpoRectangle.Stroke = color;
                        lpoBorder.Background = color;
                        //lpoSelectionIndicatorEllipse.Fill = color;
                    })
                    .Subscribe()
                    .DisposeWith(d);

                CastedViewModel.SalesPrePrintDto.WhenAnyValue(dto => dto.FilteredBySite, dto => dto.FilteredByMaterial)
                    .Select(x => (x.Item1 || x.Item2) ? new SolidColorBrush(Color.FromArgb(255, 205, 0, 0)) : new SolidColorBrush(Color.FromArgb(255, 203, 203, 203)))
                    .Do(color =>
                    {
                        siteAndMaterialRectangle.Stroke = color;
                        siteAndMaterialBorder.Background = color;
                        //siteAndMaterialSelectionIndicatorEllipse.Fill = color;
                    })
                    .Subscribe()
                    .DisposeWith(d);

                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ => showFilterPrintButton.IsEnabled = ViewModel.ActiveList.Select(s => s.CompanyId).Distinct().Count() == 1)
                    .Subscribe()
                    .DisposeWith(d);

                ViewModel.SummaryItem = new Sale();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Price = ViewModel.ActiveList.Sum(i => i.Price);
                        ViewModel.SummaryItem.DeliveryFee = ViewModel.ActiveList.Sum(i => i.DeliveryFee);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
