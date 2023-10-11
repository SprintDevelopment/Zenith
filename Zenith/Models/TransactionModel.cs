using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class TransactionModel : Model
    {
        public CashStates CashState { get; set; }

        public TransactionModel()
        {
            this.ValidationRule(vm => vm.CashState, cs => cs != CashStates.DontCare, "Select cash state");
        }
    }
}
