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

        [Reactive]
        public short? ParentOutgoCategoryId { get; set; }

        [ForeignKey(nameof(ParentOutgoCategoryId))]
        [Reactive]
        public virtual OutgoCategory Parent { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public CostCenters CostCenter { get; set; }

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
            return Title;
        }
    }
}
