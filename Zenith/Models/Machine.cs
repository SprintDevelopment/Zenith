using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "ماشین", MultipleName = "ماشین ها")]
    public class Machine : Model
    {
        [Key]
        [Reactive]
        public int MachineId { get; set; }

        [Required(ErrorMessage = "نام ماشین را وارد کنید")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        [Reactive]
        public int Capacity { get; set; }

        [Range(1, int.MaxValue)]
        [Reactive]
        public long DefaultDeliveryFee { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Machine()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "نام ماشین را وارد کنید");
            this.ValidationRule(vm => vm.Capacity, volume => volume > 0, "ظرفیت ماشین باید عددی بزرگتر از صفر باشد");
            this.ValidationRule(vm => vm.DefaultDeliveryFee, ddf => ddf > 0, "هزینه پیشفرض تحویل برای این ماشین باید عددی بزرگتر از صفر باشد");
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
