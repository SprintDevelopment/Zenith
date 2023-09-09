using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for ChequePage.xaml
    /// </summary>
    public partial class ChequePage : BaseCreateOrUpdatePage<Cheque>
    {
        public ChequePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Cheque>(new ChequeRepository());

            this.WhenActivated(d =>
            {
            });
        }
    }
}
