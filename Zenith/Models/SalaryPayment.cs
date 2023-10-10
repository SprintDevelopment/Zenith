using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class SalaryPayment : TransactionModel
    {
        [Key]
        [Reactive]
        public int SalaryPaymentId { get; set; }

        [Reactive]
        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        [Reactive]
        public virtual Person Personnel { get; set; }

        [Reactive]
        public float PaidValue { get; set; }

        [Reactive]
        public float Credit { get; set; }

        [Reactive]
        public CostCenters CostCenter { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public SalaryPayment()
        {
            CashState = CashStates.Cash;

            this.ValidationRule(vm => vm.PersonId, pi => pi > 0, "Select personnel");
            this.ValidationRule(vm => vm.PaidValue, value => value > 0, "Paid value must be greater than 0");
        }

        public override string ToString()
        {
            return $"{Personnel?.FullName} ({PaidValue:n2})";
        }
    }
}
