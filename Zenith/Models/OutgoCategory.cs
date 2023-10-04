using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class OutgoCategory : Model
    {
        [Key]
        [Reactive]
        public short OutgoCategoryId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public CostCenters CostCenter { get; set; }

        [Reactive]
        public float ReservedAmount { get; set; }

        [Reactive]
        public float ApproxPriceForEach { get; set; }

        [Reactive]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        public string CountUnitTitle { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public OutgoCategory()
        {
            this.ValidationRule(vm => vm.Title, title => !title.IsNullOrWhiteSpace(), "Enter title");
        }

        public override string ToString()
        {
            return Title + (ReservedAmount != 0 ? $" ({ReservedAmount:n2} {CountUnitTitle})" : "");
        }
    }
}
