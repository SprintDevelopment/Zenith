using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Outgo : OutgoBase
    {
        [Reactive]
        public int? RelatedOutgoPlusTransportId { get; set; }

        [Reactive]
        public int? MachineId { get; set; }

        [Reactive]
        public float MachineIncomeValue { get; set; }

        [NotMapped]
        public float TotalOutgoValue { get; set; }

        public Outgo()
        {
            this.WhenAnyValue(m => m.Value, m => m.MachineIncomeValue)
                .Select(x => x.Item1 + x.Item2)
                .BindTo(this, m => m.TotalOutgoValue);

            this.WhenAnyValue(m => m.OutgoType)
                .Skip(1)
                .Do(ot =>
                {
                    if (ot == OutgoTypes.DirectIncludeTransportation)
                    {
                        this.ValidationRule(vm => vm.MachineId, ci => ci > 0, "Select machine");
                        this.ValidationRule(vm => vm.MachineIncomeValue, value => value > 0, "Outgo value for machine must be greater than 0");
                    }
                }).Subscribe();

        }
    }
}
