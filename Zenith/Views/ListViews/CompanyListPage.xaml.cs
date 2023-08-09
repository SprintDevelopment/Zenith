using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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

                headerGrid.ColumnDefinitions.OfType<ColumnDefinition>().Skip(1)
                    .Select((rd, i) =>
                    {
                        var colSeparatorRect = new Rectangle { Stroke = Brushes.Red, HorizontalAlignment = HorizontalAlignment.Right, StrokeThickness = 0.5 };
                        colSeparatorRect.SetValue(Grid.RowSpanProperty, 2);
                        colSeparatorRect.SetValue(Grid.ColumnProperty, i);
                        headerGrid.Children.Add(colSeparatorRect);
                        return i;
                    }).ToList();
            });
        }
    }
}
