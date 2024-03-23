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
    public class TaxReportSearchModel : BaseDto
    {
        [Reactive]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Reactive]
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
}
