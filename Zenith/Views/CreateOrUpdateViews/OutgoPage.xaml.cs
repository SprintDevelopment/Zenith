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
    /// Interaction logic for OutgoCategoryPage.xaml
    /// </summary>
    public partial class OutgoPage : BaseCreateOrUpdatePage<Outgo>
    {
        public OutgoPage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Outgo>(new OutgoRepository());

            this.WhenActivated(d =>
            {
                outgoCategoryTreeView.ViewModel.ItemsSource = new OutgoCategoryRepository().All();

                this.Bind(ViewModel, vm => vm.PageModel.OutgoCategory, v => v.outgoCategoryTreeView.ViewModel.SelectedItem).DisposeWith(d);
            });
        }
    }
}
