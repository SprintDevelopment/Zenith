using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;

namespace Zenith.Models
{
    [Model(SingleName = "هزینه", MultipleName = "هزینه ها")]
    public class Outgo : Model
    {
        [Key]
        [Reactive]
        public int OutgoId { get; set; }

        [Reactive]
        public short OutgoCategoryId { get; set; }

        [ForeignKey(nameof(OutgoCategoryId))]
        [Reactive]
        public OutgoCategory OutgoCategory { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Reactive]
        public long Value { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Outgo()
        {
            this.ValidationRule(vm => vm.OutgoCategory, oc => oc is not null, "دسته هزینه باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.Value, value => value > 0, "میزان هزینه باید بیشتر از 0 باشد");
        }

        public override string ToString()
        {
            return $"{OutgoCategory?.Title} ({DateTime})";
        }
    }
}
