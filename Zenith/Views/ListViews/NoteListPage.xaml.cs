using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Zenith.Assets.Extensions;
using Zenith.Models;
using Zenith.Models.SearchModels;

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

            //Initialize(new NotePage(), new NoteRepository(), searchModel, dynamicFilter);
        }
    }
}
