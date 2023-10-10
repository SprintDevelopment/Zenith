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
    public class CashListViewModel : BaseListViewModel<Cash>
    {
        public CashListViewModel(CashRepository repository, SearchBaseDto searchModel, IObservable<Func<Cash, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Cashes)
        {
            AddNewCommand = ReactiveCommand.CreateFromObservable<MoneyTransactionTypes, Unit>(moneyTransactionType =>
                CreateCommand.Execute()
                .Do(_ => CreateUpdatePage.ViewModel.PageModel.MoneyTransactionType = moneyTransactionType));

        }

        public ReactiveCommand<MoneyTransactionTypes, Unit> AddNewCommand { get; set; }
    }
}
