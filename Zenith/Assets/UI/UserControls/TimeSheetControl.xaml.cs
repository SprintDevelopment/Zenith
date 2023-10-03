using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zenith.Assets.UI.Helpers;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using Zenith.Assets.Extensions;
using System.Globalization;
using System.Collections.ObjectModel;
using Zenith.Assets.UI.BaseClasses;
using DynamicData.Binding;
using DynamicData;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TimeSheetControl.xaml
    /// </summary>
    public partial class TimeSheetControl : ActivatableUserControl, IViewFor<TimeSheetViewModel>
    {
        public TimeSheetControl()
        {
            InitializeComponent();
            ViewModel = new TimeSheetViewModel();

            this.WhenActivated(d =>
            {
                ViewModel.WhenAnyValue(vm => vm.DateTime)
                    .Do(dt =>
                    {
                        ViewModel.Year = dt.Year;
                        ViewModel.Month = dt.Month;
                        ViewModel.Day = dt.Day;
                    }).Subscribe().DisposeWith(d);

                Observable.FromEventPattern(preButton, nameof(Button.Click))
                    .Do(_ => ViewModel.DateTime = ViewModel.DateTime.AddMonths(-1))
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(nextButton, nameof(Button.Click))
                    .Do(_ => ViewModel.DateTime = ViewModel.DateTime.AddMonths(1))
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.HighligtDates)
                    .WhereNotNull()
                    .SelectMany(h => h.ToObservableChangeSet().QueryWhenChanged())
                    .Select(h => ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month)
                            .Select(x => new { year = x.Item1, month = x.Item2, offDays = h })
                            .Where(x => x.month > 0 && x.year > 0))
                    .Switch()
                    .Do(x =>
                    {
                        var dayOfWeek = (short)new DateTime((int)x.year, (int)x.month, 1).DayOfWeek;

                        daysGrid.Children.OfType<JTimeSheetDayButton>()
                        .Select((button, i) =>
                        {
                            return new { btn = button, date = new DateTime(x.year, x.month, 1).AddDays(i - dayOfWeek) };
                        })
                        .Select(y =>
                        {
                            y.btn.Opacity = y.date.Month == x.month ? 1 : 0.3;
                            y.btn.Content = y.date.Day.ToString();
                            y.btn.Tag = y.date;
                            y.btn.IsAbsent = x.offDays.Contains(y.date);
                            return y;
                        }).ToList();
                    }).Subscribe().DisposeWith(d);

                daysGrid.Children.OfType<JTimeSheetDayButton>()
                    .ToList()
                    .ForEach(btn => btn.Click += (s, e) => { DayClicked?.Invoke(this, (DateTime)((JTimeSheetDayButton)s).Tag); });

                var dayOfWeek = (short)DateTime.Today.AddDays(-1 * DateTime.Today.Day + 1).DayOfWeek % 7;

                dayIndentifierRect.SetValue(Grid.RowProperty, (DateTime.Today.Day + dayOfWeek - 1) / 7 + 1);
                dayIndentifierRect.SetValue(Grid.ColumnProperty, (DateTime.Today.Day + dayOfWeek - 1) % 7);

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month)
                    .Select(x => (x.Item1 == DateTime.Today.Year && x.Item2 == DateTime.Today.Month).Viz())
                    .BindTo(this, v => v.dayIndentifierRect.Visibility)
                    .DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month)
                    .Select(x => $"{DateTimeFormatInfo.InvariantInfo.MonthNames[x.Item2 - 1]} {x.Item1}")
                    .BindTo(this, v => v.yearMonthTextBlock.Text)
                    .DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TimeSheetViewModel)value; }
        }

        public TimeSheetViewModel ViewModel { get; set; }

        public event EventHandler<DateTime> DayClicked;
    }
    public class TimeSheetViewModel : ReactiveObject
    {
        [Reactive]
        public int Year { get; set; }

        [Reactive]
        public int Month { get; set; }

        [Reactive]
        public int Day { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Today;

        [Reactive]
        public ObservableCollection<DateTime> HighligtDates { get; set; } = new ObservableCollection<DateTime>();
    }
}
