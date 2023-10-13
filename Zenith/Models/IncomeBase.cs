using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class IncomeBase : TransactionModel
    {
        [Key]
        [Reactive]
        public int IncomeId { get; set; }

        [Reactive]
        public short IncomeCategoryId { get; set; }

        [ForeignKey(nameof(IncomeCategoryId))]
        [Reactive]
        public virtual IncomeCategory IncomeCategory { get; set; }

        [Reactive]
        public short? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company? Company { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string FactorNumber { get; set; } = string.Empty;

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Reactive]
        public float Value { get; set; }

        [Reactive]
        public float Amount { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public IncomeBase()
        {
            this.ValidationRule(vm => vm.IncomeCategoryId, oci => oci > 0, "Select income category");
            this.ValidationRule(vm => vm.Value, value => value > 0, "Income value must be greater than 0");
            this.ValidationRule(vm => vm.Amount, amount => amount > 0, "Amount must be greater than 0");
        }

        public override string ToString()
        {
            return $"{IncomeCategory?.Title} ({DateTime})";
        }
    }
}
