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
    /// Interaction logic for NotePage.xaml
    /// </summary>
    public partial class NotePage : BaseCreateOrUpdatePage<Note>
    {
        public NotePage()
        {
            InitializeComponent();

            ViewModel = new BaseCreateOrUpdateViewModel<Note>(new NoteRepository());

            // Should be out of this.WhenActivated !!
            notifyTypeComboBox.ItemsSource = typeof(NotifyTypes).ToCollection();

            this.WhenActivated(d =>
            {
                ViewModel.WhenAnyValue(vm => vm.PageModel)
                    .Select(pm => pm.WhenAnyValue(pm => pm.NotifyType))
                    .Switch()
                    .Do(nt => notifyDateTimePicker.Visibility = (nt == NotifyTypes.FooterNotify).Viz())
                    .Subscribe().DisposeWith(d);
            });
        }
    }
}
