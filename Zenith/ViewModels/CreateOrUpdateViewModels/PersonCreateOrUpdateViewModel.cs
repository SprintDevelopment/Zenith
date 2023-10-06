using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using System.Reactive;
using Zenith.Assets.Utils;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class PersonCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<Person>
    {
        public PersonCreateOrUpdateViewModel(Repository<Person> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            var personnelAbsenceCreateOrUpdatePage = new PersonnelAbsencePage();
            var personnelOvertimeCreateOrUpdatePage = new PersonnelOvertimePage();

            IDisposable createUpdateDisposable = null;
            AddNewPersonnelAbsenceCommand = ReactiveCommand.Create<DateTime>(dateTime =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = personnelAbsenceCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        PageModel.PersonnelAbsences.AddRange(changeSet);
                    }
                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                personnelAbsenceCreateOrUpdatePage.ViewModel.PrepareCommand.Execute().Subscribe();

                //Important
                personnelAbsenceCreateOrUpdatePage.ViewModel.PageModel.PersonId = PageModel.PersonId;
                personnelAbsenceCreateOrUpdatePage.ViewModel.PageModel.DateTime = dateTime;

                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(personnelAbsenceCreateOrUpdatePage).Subscribe();
            });

            UpdatePersonnelAbsenceCommand = ReactiveCommand.Create<PersonnelAbsence>(personnelAbsenceToUpdate =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = personnelAbsenceCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        MapperUtil.Mapper.Map(changeSet.FirstOrDefault(), personnelAbsenceToUpdate);
                    }
                    else
                    {
                        PageModel.PersonnelAbsences.Remove(personnelAbsenceToUpdate);
                    }

                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                personnelAbsenceCreateOrUpdatePage.ViewModel.PrepareCommand.Execute(personnelAbsenceToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(personnelAbsenceCreateOrUpdatePage).Subscribe();
            });

            AddNewPersonnelOvertimeCommand = ReactiveCommand.Create<DateTime>(dateTime =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = personnelOvertimeCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        PageModel.PersonnelOvertimes.AddRange(changeSet);
                    }
                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                personnelOvertimeCreateOrUpdatePage.ViewModel.PrepareCommand.Execute().Subscribe();

                //Important
                personnelOvertimeCreateOrUpdatePage.ViewModel.PageModel.PersonId = PageModel.PersonId;
                personnelOvertimeCreateOrUpdatePage.ViewModel.PageModel.DateTime = dateTime;

                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(personnelOvertimeCreateOrUpdatePage).Subscribe();
            });

            UpdatePersonnelOvertimeCommand = ReactiveCommand.Create<PersonnelOvertime>(personnelOvertimeToUpdate =>
            {
                createUpdateDisposable?.Dispose();

                createUpdateDisposable = personnelOvertimeCreateOrUpdatePage.ViewModel.ReturnCommand.Subscribe(changeSet =>
                {
                    if (!changeSet.IsNullOrEmpty())
                    {
                        MapperUtil.Mapper.Map(changeSet.FirstOrDefault(), personnelOvertimeToUpdate);
                    }
                    else
                    {
                        PageModel.PersonnelOvertimes.Remove(personnelOvertimeToUpdate);
                    }

                    App.MainViewModel.SecondCreateUpdatePageReturnedCommand.Execute().Subscribe();
                });

                personnelOvertimeCreateOrUpdatePage.ViewModel.PrepareCommand.Execute(personnelOvertimeToUpdate.GetKeyPropertyValue()).Subscribe();
                App.MainViewModel.ShowSecondCreateUpdatePageCommand.Execute(personnelOvertimeCreateOrUpdatePage).Subscribe();
            });
        }

        public ReactiveCommand<DateTime, Unit> AddNewPersonnelAbsenceCommand { get; set; }
        public ReactiveCommand<PersonnelAbsence, Unit> UpdatePersonnelAbsenceCommand { get; set; }

        public ReactiveCommand<DateTime, Unit> AddNewPersonnelOvertimeCommand { get; set; }
        public ReactiveCommand<PersonnelOvertime, Unit> UpdatePersonnelOvertimeCommand { get; set; }
    }
}
