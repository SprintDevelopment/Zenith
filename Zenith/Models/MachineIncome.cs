using ReactiveUI.Validation.Extensions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI.Fody.Helpers;

namespace Zenith.Models
{
    public class MachineIncome : IncomeBase
    {
        [Reactive]
        public int MachineId { get; set; }

        [ForeignKey(nameof(MachineId))]
        [Reactive]
        public virtual Machine Machine { get; set; }

        public int? RelatedOutgoPlusTransportId { get; set; }

        public MachineIncome()
        {
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "Select related machine");
        }

        public override string ToString()
        {
            return $"{IncomeCategory?.Title} for {Machine?.Name} ({DateTime})";
        }

    }
}
