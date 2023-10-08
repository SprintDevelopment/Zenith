using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for SalaryPaymentPage.xaml
    /// </summary>
    public partial class SalaryPaymentPage : BaseCreateOrUpdatePage<SalaryPayment>
    {
        public SalaryPaymentPage()
        {
            InitializeComponent();

            ViewModel = new SalaryPaymentCreateOrUpdateViewModel(new SalaryPaymentRepository());

            costCenterComboBox.ItemsSource = typeof(CostCenters).ToCollection();

            this.WhenActivated(d =>
            {
                personnelComboBox.ItemsSource = new PersonRepository().All().ToList();

                ViewModel.PageModel.WhenAnyValue(pm => pm.PersonId)
                    .Select(pi => (pi > 0).Viz())
                    .BindTo(this, v => v.statisticsGrid.Visibility)
                    .DisposeWith(d);
            });
        }
    }
}
