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
    public class MachineOutgo : OutgoBase
    {
        [Reactive]
        public int MachineId { get; set; }

        [ForeignKey(nameof(MachineId))]
        [Reactive]
        public virtual Machine Machine { get; set; }

        public MachineOutgo()
        {
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "Select related machine");
        }

        public override string ToString()
        {
            return $"{OutgoCategory?.Title} for {Machine?.Name} ({DateTime})";
        }

    }
}
