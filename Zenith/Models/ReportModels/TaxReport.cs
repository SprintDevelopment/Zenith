using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;
using System.Reactive.Linq;

namespace Zenith.Models.ReportModels
{
    public class TaxReport : ReportModel
    {
        [Reactive]
        public float TaxedSaleAmount { get; set; }

        [Reactive]
        public float TaxedBuyAmount { get; set; }

        [Reactive]
        public float TaxedOutgoAmount { get; set; }

        [Reactive]
        public float OverallAmount { get; set; }

        [Reactive]
        public float Tax { get; set; }

        public TaxReport()
        {
            this.WhenAnyValue(m => m.TaxedSaleAmount, m => m.TaxedBuyAmount, m => m.TaxedOutgoAmount)
                .Select(x => x.Item1 - (x.Item2 + x.Item3))
                .BindTo(this, m => m.OverallAmount);

            this.WhenAnyValue(m => m.OverallAmount)
                .Select(o => o * 0.05)
                .BindTo(this, m => m.Tax);
        }
    }
}
