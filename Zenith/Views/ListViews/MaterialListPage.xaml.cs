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
    /// Interaction logic for MaterialListPage.xaml
    /// </summary>
    public partial class MaterialListPage : BaseListPage<Material>
    {
        public MaterialListPage()
        {
            InitializeComponent();
            var searchModel = new MaterialSearchModel();

            IObservable<Func<Material, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(SynchronizationContext.Current)
                .Select(s => new { Title = s }).Select(s => new Func<Material, bool>(p => true));

            ViewModel = new BaseListViewModel<Material>(new MaterialRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new MaterialPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;

                headerGrid.ColumnDefinitions.OfType<ColumnDefinition>().Skip(1)
                    .Select((rd, i) =>
                    {
                        var colSeparatorRect = new Rectangle { Stroke = Brushes.WhiteSmoke, HorizontalAlignment = HorizontalAlignment.Right, StrokeThickness = 0.5 };
                        colSeparatorRect.SetValue(Grid.RowSpanProperty, 2);
                        colSeparatorRect.SetValue(Grid.ColumnProperty, i);
                        headerGrid.Children.Add(colSeparatorRect);
                        return i;
                    }).ToList();
            });
        }
    }
}
