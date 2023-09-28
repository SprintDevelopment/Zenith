using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels
{
    public class SaleProfitReport : ReportModel
    {
        [Reactive]
        public int Year { get; set; }

        [Reactive]
        public Months Month { get; set; }

        [Reactive]
        public string MaterialName { get; set; }

        [Reactive]
        public float SoldCount { get; set;}

        [Reactive]
        public float SoldPrice { get; set; }

        [Reactive]
        public float BoughtPrice { get; set;}

        public float Profit { get => SoldPrice - BoughtPrice; }

        public bool? ProfitState { get => Profit != 0 ? Profit > 0 : (bool?)null; }

        [Reactive]
        public bool? IsValid { get; set; }
    }
}
