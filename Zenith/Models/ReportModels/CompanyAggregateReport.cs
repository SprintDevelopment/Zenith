using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels
{
    public class CompanyAggregateReport : ReportModel
    {
        [Reactive]
        public string CompanyName { get; set; }

        [Reactive]
        public int Year { get; set; }

        [Reactive]
        public Months Month { get; set; }

        [Reactive]
        public int SiteId { get; set; }

        [Reactive]
        public string SiteName { get; set; }

        [Reactive]
        public string InvoiceNo { get; set; }

        [Reactive]
        public float TotalAmount { get; set; }
    }
}
