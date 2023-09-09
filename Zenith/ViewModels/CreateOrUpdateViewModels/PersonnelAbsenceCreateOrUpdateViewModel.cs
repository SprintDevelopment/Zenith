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

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class PersonnelAbsenceCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<PersonnelAbsence>
    {
        public PersonnelAbsenceCreateOrUpdateViewModel(Repository<PersonnelAbsence> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            DeletePersonnelAbsenceCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                repository.Remove(PageModel);
                ReturnCommand.Execute().Subscribe();
            });
        }
        public ReactiveCommand<Unit, Unit> DeletePersonnelAbsenceCommand { get; set; }
    }
}
