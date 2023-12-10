using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Company : Model
    {
        [Key]
        [Reactive]
        public short CompanyId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public bool  IsTaxPayer { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string TaxRegistrationNumber { get; set; } = string.Empty;

        [Reactive]
        public CompanyTypes CompanyType { get; set; }

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

        [Reactive]
        public float InitialCreditValue { get; set; }

        [Reactive]
        public float CreditValue { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Company()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "Enter name");
            //this.ValidationRule(vm => vm.TaxRegistrationNumber, trn => !trn.IsNullOrWhiteSpace(), "Enter trn");
            //this.ValidationRule(vm => vm.NotifyType, notifyType => notifytType > 0, "روش(های) اطلاعرسانی را انتخاب کنید");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
