using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;

namespace Zenith.ViewModels.ListViewModels
{
    public class BuyListViewModel : BaseListViewModel<Buy>
    {
        public BuyListViewModel(Repository<Buy> repository, SearchBaseDto searchModel, IObservable<Func<Buy, bool>> criteria)
            : base(repository, searchModel, criteria, PermissionTypes.Buys)
        {
            PrintCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                WordUtil.PrintBuyFactor(ActiveList.Where(s => s.IsSelected).Select(s => repository.Single(s.BuyId)).ToArray());
            }, this.WhenAnyValue(vm => vm.SelectionMode)
                .Select(selectionMode => selectionMode != SelectionModes.NoItemSelected));
        }

        public ReactiveCommand<Unit, Unit> PrintCommand { get; set; }
    }
}
