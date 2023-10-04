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
    /// Interaction logic for MachineOutgoListPage.xaml
    /// </summary>
    public partial class MachineOutgoListPage : BaseListPage<MachineOutgo>
    {
        public MachineOutgoListPage()
        {
            InitializeComponent();
            var searchModel = new MachineOutgoSearchModel();

            IObservable<Func<MachineOutgo, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<MachineOutgo, bool>(mo =>
                    (s.MachineId == 0 || mo.MachineId == s.MachineId) &&
                    (s.OutgoCategoryId == 0 || mo.OutgoCategoryId == s.OutgoCategoryId) &&
                    (s.CompanyId == 0 || mo.CompanyId == s.CompanyId)));

            ViewModel = new MachineOutgoListViewModel(new MachineOutgoRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new MachineOutgoPage()
            };

            this.WhenActivated(d => 
            { 
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                ViewModel.SummaryItem = new MachineOutgo();
                Observable.FromEventPattern(ViewModel.ActiveList, nameof(ViewModel.ActiveList.CollectionChanged))
                    .Throttle(TimeSpan.FromMicroseconds(500))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(_ =>
                    {
                        ViewModel.SummaryItem.Value = ViewModel.ActiveList.Where(mo => mo.OutgoType != OutgoTypes.UseConsumables).Sum(i => i.Value);
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
