using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;

namespace Zenith.ViewModels.ListViewModels
{
    public class SalaryPaymentListViewModel : BaseListViewModel<SalaryPayment>
    {
        public SalaryPaymentListViewModel(Repository<SalaryPayment> repository, SearchBaseDto searchModel, IObservable<Func<SalaryPayment, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.SalaryPayments)
        {
            PrintReceiptCommand = ReactiveCommand.CreateRunInBackground<SalaryPayment>(payment =>
            {
                WordUtil.PrintSalaryReceipt(repository.Single(payment.SalaryPaymentId));
            });
        }
       
        public ReactiveCommand<SalaryPayment, Unit> PrintReceiptCommand { get; set; }
    }
}
