using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
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

            IObservable<Func<Note, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Subject)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(SynchronizationContext.Current)
                .Select(subject => new Func<Note, bool>(p => subject.IsNullOrWhiteSpace() || p.Subject.Contains(subject)));

            ViewModel = new BaseListViewModel<Note>(new NoteRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new NotePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}
