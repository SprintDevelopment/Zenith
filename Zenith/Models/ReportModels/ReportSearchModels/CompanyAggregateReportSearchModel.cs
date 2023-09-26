using ReactiveUI.Fody.Helpers;
using System;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels.ReportSearchModels
{
    public class CompanyAggregateReportSearchModel : BaseDto
    {
        [Reactive]
        public short CompanyId { get; set; }
        
        [Reactive]
        public int SiteId { get; set; }

        [Reactive]
        public int Year { get; set; } = DateTime.Today.Year;
        
        [Reactive]
        public Months Month { get; set; } = (Months)DateTime.Today.Month;
    }
}
