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
    public class OutgoBase : Model
    {
        [Key]
        [Reactive]
        public int OutgoId { get; set; }

        [Reactive]
        public short OutgoCategoryId { get; set; }

        [ForeignKey(nameof(OutgoCategoryId))]
        [Reactive]
        public virtual OutgoCategory OutgoCategory { get; set; }

        [Reactive]
        public OutgoTypes OutgoType { get; set; }

        [Reactive]
        public short? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company? Company { get; set; }

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

        public OutgoBase()
        {
            this.ValidationRule(vm => vm.OutgoCategoryId, oci => oci > 0, "Select outgo category");
            this.ValidationRule(vm => vm.Value, value => value > 0, "Outgo value must be greater than 0");
            this.ValidationRule(vm => vm.Amount, amount => amount > 0, "Amount must be greater than 0");

            this.WhenAnyValue(m => m.OutgoCategory)
                .WhereNotNull()
                .Do(oc => OutgoCategoryId = oc.OutgoCategoryId)
                .Subscribe();
        }

        public override string ToString()
        {
            return $"{OutgoCategory?.Title} ({DateTime})";
        }
    }
}
