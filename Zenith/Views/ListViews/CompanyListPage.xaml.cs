using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Models.SearchModels;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using DynamicData.Binding;
using System.Reactive.Disposables;
using DynamicData;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for CompanyListPage.xaml
    /// </summary>
    public partial class CompanyListPage : BaseListPage<Company>
    {
        public CompanyListPage()
        {
            InitializeComponent();
            var searchModel = new CompanySearchModel();

            IObservable<Func<Company, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Company, bool>(c =>
                    (s.Name.IsNullOrWhiteSpace() || c.Name.Contains(s.Name))));

            ViewModel = new BaseListViewModel<Company>(new CompanyRepository(), searchModel, dynamicFilter, PermissionTypes.Companies)
            {
                CreateUpdatePage = new CompanyPage()
            };

            this.WhenActivated(d =>
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new Company();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.CreditValue = ViewModel.ActiveList.Sum(i => i.CreditValue);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
