using ReactiveUI;
using System.Reactive.Disposables;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : BaseCreateOrUpdatePage<Company>
    {
        public CompanyPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Company>(new CompanyRepository());

            // Should be out of this.WhenActivated !!
            companyTypeComboBox.ItemsSource = typeof(CompanyTypes).ToCollection();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsNew, v => v.initialCreditValueTextBox.Visibility, isNew => isNew.Viz()).DisposeWith(d);
            });
        }
    }
}
