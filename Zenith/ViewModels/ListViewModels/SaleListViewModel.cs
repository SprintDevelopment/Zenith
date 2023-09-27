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
    public class SaleListViewModel : BaseListViewModel<Sale>
    {
        public SaleListViewModel(Repository<Sale> repository, SearchBaseDto searchModel, IObservable<Func<Sale, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Sales)
        {
            PrintFactorCommand = ReactiveCommand.CreateRunInBackground<Sale>(sale =>
            {
                WordUtil.PrintFactor(IncludeTRN, new SaleRepository().Single(sale.SaleId));
            });

            PrintAggregateFactorCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                WordUtil.PrintFactor(IncludeTRN, ActiveList.Where(item => item.IsSelected).Select(s => new SaleRepository().Single(s.SaleId)).ToArray());
            }, this.WhenAnyValue(vm => vm.SelectionMode)
                .Select(selectionMode => selectionMode != SelectionModes.NoItemSelected));

        }
        public ReactiveCommand<Sale, Unit> PrintFactorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PrintAggregateFactorCommand { get; set; }

        [Reactive]
        public bool IncludeTRN { get; set; }
    }
}
