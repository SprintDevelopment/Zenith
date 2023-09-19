using ReactiveUI;
using System;
using System.Linq;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using System.Reactive;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class PersonnelAbsenceCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<PersonnelAbsence>
    {
        public PersonnelAbsenceCreateOrUpdateViewModel(Repository<PersonnelAbsence> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            DeletePersonnelAbsenceCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                repository.RemoveRange(Enumerable.Empty<PersonnelAbsence>().Append(PageModel));
                ReturnCommand.Execute().Subscribe();
            });
        }
        public ReactiveCommand<Unit, Unit> DeletePersonnelAbsenceCommand { get; set; }
    }
}
