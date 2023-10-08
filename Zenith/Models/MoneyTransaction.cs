using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI;
using System.Reactive.Linq;

namespace Zenith.Models
{
    public class MoneyTransaction : Model
    {
        [Reactive]
        public TransferDirections TransferDirection { get; set; }

        [NotMapped]
        [Reactive]
        public bool? TransferDirectionPlusValueToNullableBool { get; set; }

        [Reactive]
        public short? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company? Company { get; set; }

        [Reactive]
        public float Value { get; set; }

        [Reactive]
        public DateTime IssueDateTime { get; set; } = DateTime.Today;

        [Reactive]
        public MoneyTransactionTypes MoneyTransactionType { get; set; }

        [Reactive]
        public CostCenters CostCenter { get; set; }

        [Reactive]
        public int RelatedEntityId { get; set; } = -1;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public MoneyTransaction()
        {
            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select related company");
            this.ValidationRule(vm => vm.Value, v => v > 0, "Value must be greater than 0");

            this.WhenAnyValue(m => m.Value, m => m.TransferDirection)
                .Select(m => m.Item1 != 0 ? m.Item2 == TransferDirections.FromCompnay : (bool?)null)
                .BindTo(this, m => m.TransferDirectionPlusValueToNullableBool);
        }
    }
}
