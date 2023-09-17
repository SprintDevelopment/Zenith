using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
    public class SaleListViewModel : BaseListViewModel<Sale>
    {
        public SaleListViewModel(Repository<Sale> repository, SearchBaseDto searchModel, IObservable<Func<Sale, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Sales)
        {
            PrintFactorCommand = ReactiveCommand.CreateRunInBackground<Sale>(sale =>
            {
                WordUtil.PrintFactor(new SaleRepository().Single(sale.SaleId));
            });

        }
        public ReactiveCommand<Sale, Unit> PrintFactorCommand { get; set; }
    }
}
