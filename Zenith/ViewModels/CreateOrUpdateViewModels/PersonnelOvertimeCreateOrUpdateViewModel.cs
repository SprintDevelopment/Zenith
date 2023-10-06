using ReactiveUI;
using System;
using System.Linq;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using System.Reactive;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class PersonnelOvertimeCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<PersonnelOvertime>
    {
        public PersonnelOvertimeCreateOrUpdateViewModel(Repository<PersonnelOvertime> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            DeletePersonnelOvertimeCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                repository.RemoveRange(Enumerable.Empty<PersonnelOvertime>().Append(PageModel));
                ReturnCommand.Execute().Subscribe();
            });
        }
        public ReactiveCommand<Unit, Unit> DeletePersonnelOvertimeCommand { get; set; }
    }
}
