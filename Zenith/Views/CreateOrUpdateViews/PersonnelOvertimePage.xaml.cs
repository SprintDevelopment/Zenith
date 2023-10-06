using ReactiveUI;
using System;
using System.Data.Common;
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
    /// Interaction logic for PersonnelOvertimePage.xaml
    /// </summary>
    public partial class PersonnelOvertimePage : BaseCreateOrUpdatePage<PersonnelOvertime>
    {
        public PersonnelOvertimePage()
        {
            InitializeComponent();

            ViewModel = new PersonnelOvertimeCreateOrUpdateViewModel(new PersonnelOvertimeRepository());
            this.WhenActivated(d =>
            {
                personComboBox.ItemsSource = new PersonRepository().All().ToList();

                amountTextBox.Focus();
            });
        }
    }
}
