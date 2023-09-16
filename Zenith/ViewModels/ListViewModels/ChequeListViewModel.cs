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
    public class ChequeListViewModel : BaseListViewModel<Cheque>
    {
        public ChequeListViewModel(ChequeRepository repository, SearchBaseDto searchModel, IObservable<Func<Cheque, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Cheques)
        {
            AddNewCommand = ReactiveCommand.CreateFromObservable<TransferDirections, Unit>(transferDirection =>
                CreateCommand.Execute()
                .Do(_ =>
                {
                    CreateUpdatePage.ViewModel.PageModel.TransferDirection = transferDirection;
                    CreateUpdatePage.ViewModel.PageModel.ChequeState = ChequeStates.NotDue;
                }));

        }

        public ReactiveCommand<TransferDirections, Unit> AddNewCommand { get; set; }
    }
}
