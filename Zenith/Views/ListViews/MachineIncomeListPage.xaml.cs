using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.SearchModels;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for MachineIncomeListPage.xaml
    /// </summary>
    public partial class MachineIncomeListPage : BaseListPage<MachineIncome>
    {
        public MachineIncomeListPage()
        {
            InitializeComponent();
            var searchModel = new MachineIncomeSearchModel();

            IObservable<Func<MachineIncome, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<MachineIncome, bool>(mo =>
                    (s.MachineId == 0 || mo.MachineId == s.MachineId) &&
                    (s.IncomeCategoryId == 0 || mo.IncomeCategoryId == s.IncomeCategoryId) &&
                    (s.CompanyId == 0 || mo.CompanyId == s.CompanyId)));

            ViewModel = new BaseListViewModel<MachineIncome>(new MachineIncomeRepository(), searchModel, dynamicFilter, PermissionTypes.MachineIncomes)
            {
                CreateUpdatePage = new MachineIncomePage()
            };

            this.WhenActivated(d => 
            { 
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new MachineIncome();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Value = ViewModel.ActiveList.Sum(i => i.Value);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
