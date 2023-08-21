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

namespace Zenith.Models
{
    [Model(SingleName = "سایت", MultipleName = "سایت ها")]
    public class Site : Model
    {
        [Key]
        [Reactive]
        public int SiteId { get; set; }

        [Required(ErrorMessage = "نام شرکت نمی تواند خالی باشد")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public short CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public Company Company { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Address { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Site()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "نام نمی تواند خالی باشد");
            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "شرکت مربوطه باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.Address, address => !address.IsNullOrWhiteSpace(), "آدرس نمی تواند خالی باشد");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
