using ReactiveUI.Validation.Extensions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Models
{
    public class MachineOutgo : Outgo
    {
        public int MachineId { get; set; }

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
