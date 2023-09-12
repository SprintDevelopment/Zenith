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

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for CashListPage.xaml
    /// </summary>
    public partial class CashListPage : BaseListPage<Cash>
    {
        public CashListPage()
        {
            InitializeComponent();
            var searchModel = new CashSearchModel();

            IObservable<Func<Cash, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title, n => n.OnlyForRefreshAfterUpdate)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new { Title = s }).Select(s => new Func<Cash, bool>(p => true));

            ViewModel = new CashListViewModel(new CashRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new CashPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
