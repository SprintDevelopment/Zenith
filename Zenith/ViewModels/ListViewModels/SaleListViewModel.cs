using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;

namespace Zenith.ViewModels.ListViewModels
{
    public class SaleListViewModel : BaseListViewModel<Sale>
    {
        public SaleListViewModel(Repository<Sale> repository, SearchBaseDto searchModel, IObservable<Func<Sale, bool>> criteria)
            : base(repository, searchModel, criteria)
        {
            ShowDeliveriesCommand = ReactiveCommand.CreateFromObservable<Sale, Unit>(sale => App.MainViewModel.ShowDeliveriesCommand.Execute(sale));
        }

        public ReactiveCommand<Sale, Unit> ShowDeliveriesCommand { get; set; }
    }
}
