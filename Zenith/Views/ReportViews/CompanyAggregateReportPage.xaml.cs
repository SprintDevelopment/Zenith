using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
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
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;
using Zenith.Repositories;
using Zenith.Repositories.ReportRepositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.ViewModels.ReportViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ReportViews
{
    /// <summary>
    /// Interaction logic for CompanyAggregateReportPage.xaml
    /// </summary>
    public partial class CompanyAggregateReportPage : BaseReportPage<CompanyAggregateReport>
    {
        public CompanyAggregateReportPage()
        {
            InitializeComponent();
            var searchModel = new CompanyAggregateReportSearchModel();

            ViewModel = new CompanyAggregateReportViewModel(new CompanyAggregateReportRepository(), searchModel, PermissionTypes.CompanyAggregateReport)
            {
                //CreateUpdatePage = new BuyPage()
            };

            monthComboBox.ItemsSource = typeof(Months).ToCollection();

            this.WhenActivated(d =>
            {
                yearComboBox.ItemsSource = Enumerable.Range(2020, 10).ToList();
                yearComboBox.SelectedValue = DateTime.Today.Year;

                companyComboBox.ItemsSource = new CompanyRepository().All().ToList();

                Observable.FromEventPattern(companyComboBox, nameof(ComboBox.SelectionChanged))
                    .Do(_ =>
                    {
                        siteComboBox.ItemsSource = new SiteRepository().FindForSearch(s => s.CompanyId == searchModel.CompanyId);
                    }).Subscribe().DisposeWith(d);

                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
