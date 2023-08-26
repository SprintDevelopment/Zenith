using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;

namespace Zenith.Models
{
    [Model(SingleName = "دسته هزینه", MultipleName = "دسته های هزینه")]
    public class OutgoCategory : Model
    {
        [Key]
        [Reactive]
        public short OutgoCategoryId { get; set; }

        [Reactive]
        public short? ParentOutgoCategoryId { get; set; }

        [ForeignKey(nameof(ParentOutgoCategoryId))]
        [Reactive]
        public OutgoCategory Parent { get; set; }

        [Required(ErrorMessage = "عنوان را وارد کنید")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public OutgoCategory()
        {
            this.ValidationRule(vm => vm.Title, title => !title.IsNullOrWhiteSpace(), "عنوان را وارد کنید");
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
