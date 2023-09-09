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
    /// Interaction logic for PersonListPage.xaml
    /// </summary>
    public partial class PersonListPage : BaseListPage<Person>
    {
        public PersonListPage()
        {
            InitializeComponent();
            var searchModel = new PersonSearchModel();

            IObservable<Func<Person, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Name, n => n.OnlyForRefreshAfterUpdate)
                .Select(x => x.Item1)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(subject => new Func<Person, bool>(p => subject.IsNullOrWhiteSpace() || p.FullName.Contains(subject)));

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
