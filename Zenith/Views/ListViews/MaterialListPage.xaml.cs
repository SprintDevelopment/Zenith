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
    /// Interaction logic for MaterialListPage.xaml
    /// </summary>
    public partial class MaterialListPage : BaseListPage<Material>
    {
        public MaterialListPage()
        {
            InitializeComponent();
            var searchModel = new MaterialSearchModel();

            IObservable<Func<Material, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Material, bool>(p =>
                    (s.Name.IsNullOrWhiteSpace() || p.Name.Contains(s.Name))));

            ViewModel = new BaseListViewModel<Material>(new MaterialRepository(), searchModel, dynamicFilter, PermissionTypes.Materials)
            {
                CreateUpdatePage = new MaterialPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
