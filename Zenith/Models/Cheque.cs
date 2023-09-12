using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Cheque : MoneyTransaction
    {
        [Key]
        [Reactive]
        public int ChequeId { get; set; }

        [Reactive]
        public string ChequeNumber { get; set; }

        [Reactive]
        public DateTime ChequeDate { get; set; }

        [Reactive]
        public ChequeStates ChequeState { get; set; } = ChequeStates.NotDue;

        public Cheque()
        {
            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select related company");
            this.ValidationRule(vm => vm.ChequeNumber, cn => !cn.IsNullOrWhiteSpace(), "Enter cheque number");
        }
    }
}
