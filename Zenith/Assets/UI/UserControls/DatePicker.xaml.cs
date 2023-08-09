using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Zenith.Assets.UI.BaseClasses;
using Zenith.Assets.UI.Helpers;

namespace Zenith.Assets.UI.UserControls
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : ActivatableUserControl, IViewFor<DatePickerViewModel>
    {
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DatePicker), new PropertyMetadata(""));

        public DatePicker()
        {
            InitializeComponent();
            ViewModel = new DatePickerViewModel();

            this.WhenActivated(d =>
            {
                var today = DateTime.Today;
                todayButton.Tag = today;
                todayButton.Content = today.ToString("dddd, dd MMMM yyyy");

                this.OneWayBind(ViewModel, vm => vm.IsInMonthlyMode, v => v.daysGrid.Visibility, i => i ? Visibility.Collapsed : Visibility.Visible).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.IsInMonthlyMode, v => v.monthsGrid.Visibility, i => i ? Visibility.Visible : Visibility.Collapsed).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.DateTime, v => v.dateTextBox.Text, dt => dt.ToString("dd/MM/yyyy")).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.IsPopupVisible, v => v.dateSelectionPopup.IsOpen).DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.DateTime)
                    .Do(dt =>
                    {
                        ViewModel.Year = dt.Year;
                        ViewModel.Month = dt.Month;
                        ViewModel.Day = dt.Day;
                    }).Subscribe().DisposeWith(d);

                Observable.FromEventPattern(yearMonthModeButton, nameof(Button.Click))
                    .Do(_ => ViewModel.IsInMonthlyMode = !ViewModel.IsInMonthlyMode)
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(showHidePopupButton, nameof(Button.Click))
                    .Do(_ => ViewModel.IsPopupVisible = !ViewModel.IsPopupVisible)
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(preButton, nameof(Button.Click))
                    .Do(_ => ViewModel.DateTime = ViewModel.DateTime.AddMonths(ViewModel.IsInMonthlyMode ? -12 : -1))
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(nextButton, nameof(Button.Click))
                    .Do(_ => ViewModel.DateTime = ViewModel.DateTime.AddMonths(ViewModel.IsInMonthlyMode ? 12 : 1))
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month, vm => vm.Day)
                    .Do(x => ViewModel.DateTime = new DateTime(x.Item1, x.Item2, x.Item3))
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month, vm => vm.IsInMonthlyMode)
                    .Select(x => new { year = x.Item1, month = x.Item2, isInMonthlyMode = x.Item3 })
                    .Do(x => yearMonthModeButton.Content = x.isInMonthlyMode ? $"{x.year}" : $"{DateTimeFormatInfo.InvariantInfo.MonthNames[x.month - 1]} {x.year}")
                    .Subscribe().DisposeWith(d);

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month)
                    .Select(x => new { year = x.Item1, month = x.Item2 })
                    .Where(x => x.month > 0 && x.year > 0)
                    .Do(x =>
                    {
                        var dayOfWeek = (short)new DateTime((int)x.year, (int)x.month, 1).DayOfWeek;

                        daysGrid.Children.OfType<Button>()
                        .Select((button, i) =>
                        {
                            return new { btn = button, date = new DateTime(x.year, x.month, 1).AddDays(i - dayOfWeek) };
                        })
                        .Select(y =>
                        {
                            y.btn.Opacity = y.date.Month == x.month ? 1 : 0.3;
                            y.btn.Content = y.date.Day.ToString();
                            y.btn.Tag = y.date;
                            return y;
                        }).ToList();
                    }).Subscribe().DisposeWith(d);

                daysGrid.Children.OfType<Button>().Append(todayButton)
                    .Select(btn => Observable.FromEventPattern(btn, nameof(btn.Click))
                    .Select(e => (DateTime)((Button)e.Sender).Tag)
                    .Do(date => { ViewModel.DateTime = date; ViewModel.IsPopupVisible = false; })
                    .Subscribe().DisposeWith(d)).ToList();

                monthsGrid.Children.OfType<Button>()
                    .Select((btn, i) => Observable.FromEventPattern(btn, nameof(btn.Click))
                    .Do(date => { ViewModel.Month = i + 1; ViewModel.IsInMonthlyMode = false; })
                    .Subscribe().DisposeWith(d)).ToList();

                ViewModel.WhenAnyValue(vm => vm.Year, vm => vm.Month, vm => vm.Day)
                    .Select(x => new { year = x.Item1, month = x.Item2, day = x.Item3 })
                    .Where(x => x.day > 0 && x.month > 0 && x.year > 0)
                    .Do(x =>
                    {
                        var dayOfWeek = ((short)new DateTime((int)x.year, (int)x.month, 1).DayOfWeek) % 7;

                        dayIndentifierRect.SetValue(Grid.RowProperty, ((int)x.day + dayOfWeek - 1) / 7 + 1);
                        dayIndentifierRect.SetValue(Grid.ColumnProperty, ((int)x.day + dayOfWeek - 1) % 7);
                    }).Subscribe().DisposeWith(d);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DatePickerViewModel)value; }
        }

        public DatePickerViewModel ViewModel { get; set; }
    }

    public class DatePickerViewModel : ReactiveObject
    {
        [Reactive]
        public int Year { get; set; }

        [Reactive]
        public int Month { get; set; }

        [Reactive]
        public int Day { get; set; }

        [Reactive]
        public bool IsPopupVisible { get; set; }

        [Reactive]
        public bool IsInMonthlyMode { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Today;
    }
}
