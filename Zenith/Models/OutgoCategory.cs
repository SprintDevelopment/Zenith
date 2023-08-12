using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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

        public short? ParentOutgoCategoryId { get; set; }

        [Required(ErrorMessage = "موضوع یادداشت نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; }

        public OutgoCategory()
        {
            this.ValidationRule(vm => vm.Title, title => !title.IsNullOrWhiteSpace(), "عنوان نمی تواند خالی باشد");
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
