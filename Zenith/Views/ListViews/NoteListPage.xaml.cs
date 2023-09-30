using DynamicData.Binding;
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

            IObservable<Func<Note, bool>> dynamicFilter = searchModel
                .WhenAnyPropertyChanged()
                .WhereNotNull()
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => new Func<Note, bool>(n => 
                    (s.Subject.IsNullOrWhiteSpace() || n.Subject.Contains(s.Subject)) &&
                    (s.NotifyType == NotifyTypes.DontCare || n.NotifyType == s.NotifyType)));

            ViewModel = new BaseListViewModel<Note>(new NoteRepository(), searchModel, dynamicFilter, PermissionTypes.Notes)
            {
                CreateUpdatePage = new NotePage()
            };

            this.WhenActivated(d => { listItemsControl.ItemsSource = ViewModel.ActiveList; });
        }
    }
}
