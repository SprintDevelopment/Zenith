﻿using ReactiveUI;
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
    /// Interaction logic for SaleProfitReportPage.xaml
    /// </summary>
    public partial class SaleProfitReportPage : BaseReportPage<SaleProfitReport>
    {
        public SaleProfitReportPage()
        {
            InitializeComponent();
            var searchModel = new SaleProfitReportSearchModel();

            ViewModel = new BaseReportViewModel<SaleProfitReport>(new SaleProfitReportRepository(), searchModel, PermissionTypes.SaleProfitReport)
            {
                //CreateUpdatePage = new BuyPage()
            };

            monthComboBox.ItemsSource = typeof(Months).ToCollection();

            this.WhenActivated(d =>
            {
                yearComboBox.ItemsSource = Enumerable.Range(2020, 10).ToList();
                yearComboBox.SelectedValue = DateTime.Today.Year;

                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
