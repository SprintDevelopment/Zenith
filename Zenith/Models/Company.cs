using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "شرکت", MultipleName = "شرکت ها")]
    public class Company : Model
    {
        [Key]
        [Reactive]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "نام شرکت نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.CALL_NUMBERS)]
        [Reactive]
        public string Tel { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.CALL_NUMBERS)]
        [Reactive]
        public string Fax { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.LARGE_STRING)]
        [Reactive]
        public string Address { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Company()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "موضوع نمی تواند خالی باشد");
            ////this.ValidationRule(vm => vm.NotifyType, notifyType => notifytType > 0, "روش(های) اطلاعرسانی را انتخاب کنید");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
