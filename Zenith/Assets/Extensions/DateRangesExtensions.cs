using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Extensions
{
    public static class DateRangesExtensions
    {
        public static bool IsInDateRange(this DateTime dateTime, DateRanges dateRange)
        {
            var daysLast = DateTime.Today.Subtract(dateTime).Days;
            var thisWeeksDaysLast = (int)DateTime.Today.DayOfWeek;
            var yearsLast = DateTime.Today.Year - dateTime.Year;
            var monthsLast = DateTime.Today.Month - dateTime.Month + yearsLast * 12;

            return dateRange switch
            {
                DateRanges.Today => daysLast == 0,
                DateRanges.Yesterday => daysLast == 1,
                DateRanges.PastThreeDays => daysLast >= 0 && daysLast <= 3,
                DateRanges.ThisWeek => daysLast >= 0 && daysLast <= thisWeeksDaysLast,
                DateRanges.PastWeek => daysLast >= 0 && daysLast <= 7,
                DateRanges.PastTwoWeeks => daysLast >= 0 && daysLast <= 14,
                DateRanges.ThisMonth => monthsLast == 0,
                DateRanges.LastMonth => monthsLast == 1,
                DateRanges.PastMonth => monthsLast >= 0 && monthsLast <= 1,
                DateRanges.PastTwoMonths => monthsLast >= 0 && monthsLast <= 2,
                DateRanges.PastThreeMonth => monthsLast >= 0 && monthsLast <= 3,
                DateRanges.ThisYear => yearsLast == 0,
                DateRanges.LastYear => yearsLast == 1,
                DateRanges.PastYear => yearsLast >= 0 && yearsLast <= 1,
            };
        }
    }
}
