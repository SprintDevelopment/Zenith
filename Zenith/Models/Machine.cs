using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "Machine", MultipleName = "Machines")]
    public class Machine : Model
    {
        [Key]
        [Reactive]
        public int MachineId { get; set; }

        [Required]
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
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "Enter name");
            this.ValidationRule(vm => vm.Capacity, volume => volume > 0, "Capacity must be greater than 0");
            this.ValidationRule(vm => vm.DefaultDeliveryFee, ddf => ddf > 0, "Default delivery fee must be greater than 0");
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
