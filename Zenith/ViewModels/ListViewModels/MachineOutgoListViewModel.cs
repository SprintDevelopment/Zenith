using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.ViewModels.ListViewModels
{
    public class MachineOutgoListViewModel : BaseListViewModel<MachineOutgo>
    {
        public MachineOutgoListViewModel(MachineOutgoRepository repository, SearchBaseDto searchModel, IObservable<Func<MachineOutgo, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.MachineOutgoes)
        {
            AddNewCommand = ReactiveCommand.CreateFromObservable<OutgoTypes, Unit>(outgoType =>
                CreateCommand.Execute()
                .Do(_ => CreateUpdatePage.ViewModel.PageModel.OutgoType = outgoType));

        }

        public ReactiveCommand<OutgoTypes, Unit> AddNewCommand { get; set; }
    }
}
