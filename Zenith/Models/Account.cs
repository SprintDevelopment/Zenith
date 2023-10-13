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
    public class Account : Model
    {
        [Key]
        [Reactive]
        public short AccountId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public CostCenters CostCenter { get; set; }

        [Reactive]
        public float CreditValue { get; set; }

        [Reactive]
        public float Balance { get; set; }

        [Reactive]
        public float ChequeBalance { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Account()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "Enter name");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
