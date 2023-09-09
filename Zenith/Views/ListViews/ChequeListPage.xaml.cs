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
    /// Interaction logic for ChequeListPage.xaml
    /// </summary>
    public partial class ChequeListPage : BaseListPage<Cheque>
    {
        public ChequeListPage()
        {
            InitializeComponent();
            var searchModel = new ChequeSearchModel();

            IObservable<Func<Cheque, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new { Title = s }).Select(s => new Func<Cheque, bool>(p => true));

            ViewModel = new BaseListViewModel<Cheque>(new ChequeRepository(), searchModel, dynamicFilter, PermissionTypes.Cheques)
            {
                CreateUpdatePage = new ChequePage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
