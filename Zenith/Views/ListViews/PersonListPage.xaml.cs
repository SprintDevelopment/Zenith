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

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for PersonListPage.xaml
    /// </summary>
    public partial class PersonListPage : BaseListPage<Person>
    {
        public PersonListPage()
        {
            InitializeComponent();
            var searchModel = new PersonSearchModel();

            IObservable<Func<Person, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Person, bool>(p => 
                    (s.Name.IsNullOrWhiteSpace() || p.FullName.Contains(s.Name)) &&
                    (s.Job == Jobs.DontCare || p.Job == s.Job) &&
                    (s.CostCenter == CostCenters.DontCare || p.CostCenter == s.CostCenter)));

            ViewModel = new BaseListViewModel<Person>(new PersonRepository(), searchModel, dynamicFilter, PermissionTypes.Personnel)
            {
                CreateUpdatePage = new PersonPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
