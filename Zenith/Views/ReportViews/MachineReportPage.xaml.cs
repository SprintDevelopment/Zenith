using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MachineReportPage.xaml
    /// </summary>
    public partial class MachineReportPage : BaseReportPage<MachineReport>
    {
        public MachineReportPage()
        {
            InitializeComponent();
            var searchModel = new MachineReportSearchModel();

            ViewModel = new BaseReportViewModel<MachineReport>(new MachineReportRepository(), searchModel, PermissionTypes.MachineReport)
            {
                //CreateUpdatePage = new BuyPage()
            };

            machineReportTypeComboBox.ItemsSource = typeof(MachineReportTypes).ToCollection();

            this.WhenActivated(d =>
            {
                machineComboBox.ItemsSource = new MachineRepository().All().ToList();

                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
