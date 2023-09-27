using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels.ReportSearchModels
{
    public class SaleProfitReportSearchModel : BaseDto
    {
        [Reactive]
        public int Year { get; set; } = DateTime.Today.Year;

        [Reactive]
        public Months Month { get; set; } = (Months)DateTime.Today.Month;
    }
}
