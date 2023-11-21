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
            AddNewCommand = ReactiveCommand.CreateFromObservable<bool, Unit>(isIndirectSale =>
                CreateCommand.Execute()
                .Do(_ => CreateUpdatePage.ViewModel.PageModel.IsIndirectSale = isIndirectSale));

            HidePrePrintGridCommand = ReactiveCommand.Create<Unit>(_ => IsInPrePrintMode = false);

            PrintFactorCommand = ReactiveCommand.CreateRunInBackground<Sale>(sale =>
            {
                WordUtil.PrintFactor(IncludeTRN, repository.Single(sale.SaleId));
            });

            PrintAggregateFactorCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                WordUtil.PrintFactor(IncludeTRN, ActiveList.Where(item => item.IsSelected).Select(s => repository.Single(s.SaleId)).ToArray());
            }, this.WhenAnyValue(vm => vm.SelectionMode)
                .Select(selectionMode => selectionMode != SelectionModes.NoItemSelected));

        }
       
        public ReactiveCommand<bool, Unit> AddNewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> HidePrePrintGridCommand { get; set; }
        public ReactiveCommand<Sale, Unit> PrintFactorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PrintAggregateFactorCommand { get; set; }

        [Reactive]
        public bool IncludeTRN { get; set; }

        [Reactive]
        public bool IsInPrePrintMode { get; set; }
    }
}
