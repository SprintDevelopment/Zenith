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

            IObservable<Func<Company, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Name)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(SynchronizationContext.Current)
                .Select(subject => new Func<Company, bool>(p => subject.IsNullOrWhiteSpace() || p.Name.Contains(subject)));

            ViewModel = new BaseListViewModel<Company>(new CompanyRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new CompanyPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
