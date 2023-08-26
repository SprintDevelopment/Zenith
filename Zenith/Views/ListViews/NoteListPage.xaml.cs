using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.SearchModels;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for NoteListPage.xaml
    /// </summary>
    public partial class NoteListPage : BaseListPage<Note>
    {
        public NoteListPage()
        {
            InitializeComponent();
            var searchModel = new NoteSearchModel();

            IObservable<Func<Note, bool>> dynamicFilter = searchModel.WhenAnyValue(n => n.Subject, n => n.NotifyType)
                .Select(x => new { subject = x.Item1, notifyType = x.Item2})
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => new Func<Note, bool>(p => 
                    (x.subject.IsNullOrWhiteSpace() || p.Subject.Contains(x.subject)) &&
                    (x.notifyType == NotifyTypes.DontCare || p.NotifyType == x.notifyType)));

            ViewModel = new BaseListViewModel<Note>(new NoteRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new NotePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}
