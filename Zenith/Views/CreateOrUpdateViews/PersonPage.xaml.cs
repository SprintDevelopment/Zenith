using DynamicData;
using DynamicData.Binding;
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
    /// Interaction logic for PersonPage.xaml
    /// </summary>
    public partial class PersonPage : BaseCreateOrUpdatePage<Person>
    {
        public PersonPage()
        {
            InitializeComponent();

            ViewModel = new PersonCreateOrUpdateViewModel(new PersonRepository());
            var CastedViewModel = (PersonCreateOrUpdateViewModel)ViewModel;

            // Should be out of this.WhenActivated !!
            jobComboBox.ItemsSource = typeof(Jobs).ToCollection();
            costCenterComboBox.ItemsSource = typeof(CostCenters).ToCollection().Where(cc => (CostCenters)cc.Value != CostCenters.Consumables);

            this.WhenActivated(d =>
            {
                ViewModel.WhenAnyValue(vm => vm.PageModel.PersonnelAbsences)
                    .SelectMany(pas => pas.ToObservableChangeSet().QueryWhenChanged())
                    .Do(pas => { timeSheetControl.ViewModel.HighligtDates = pas.Select(pa => pa.DateTime).ToObservableCollection(); })
                    .Subscribe().DisposeWith(d);

                // temp hack
                ViewModel.PageModel.PersonnelAbsences.Add(new PersonnelAbsence { DateTime = DateTime.Today.AddYears(-10) }); 
                timeSheetControl.DayClicked += (s, e) =>
                {
                    var prePersonnelAbsence = ViewModel.PageModel.PersonnelAbsences.FirstOrDefault(item => item.DateTime == e);
                    if (prePersonnelAbsence is null)
                        CastedViewModel.AddNewPersonnelAbsenceCommand.Execute(e).Subscribe();
                    else
                        CastedViewModel.UpdatePersonnelAbsenceCommand.Execute(prePersonnelAbsence).Subscribe();
                };
            });
        }
    }
}
