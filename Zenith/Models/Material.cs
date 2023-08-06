using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "متریال", MultipleName = "متریال ها")]
    public class Material : Model
    {
        [Key]
        [Reactive]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "نام متریال نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Material()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "نام متریال نمی تواند خالی باشد");
            ////this.ValidationRule(vm => vm.NotifyType, notifyType => notifytType > 0, "روش(های) اطلاعرسانی را انتخاب کنید");
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
