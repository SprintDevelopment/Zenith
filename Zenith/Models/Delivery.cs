using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "تحویل", MultipleName = "تحویل ها")]
    public class Delivery : Model
    {
        [Key]
        [Reactive]
        public long DeliveryId { get; set; }

        [Reactive]
        public long SaleItemId { get; set; }

        [ForeignKey(nameof(SaleItemId))]
        [Reactive]
        public virtual SaleItem SaleItem { get; set; }

        [Reactive]
        public int Count { get; set; }

        [Reactive]
        public int MachineId { get; set; }

        [ForeignKey(nameof(MachineId))]
        [Reactive]
        public virtual Machine Machine { get; set; }

        [NotMapped]
        [Reactive]
        public bool InfoIsRelatedToMachine { get; set; } = true;

        [Reactive]
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        [Reactive]
        public virtual Person Driver { get; set; }

        [Required]
        [MaxLength(LengthConstants.SMALL_STRING)]
        [Reactive]
        public string DeliveryNumber { get; set; } = string.Empty;

        [Reactive]
        public long DeliveryFee { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Delivery()
        {
            this.ValidationRule(vm => vm.DeliveryNumber, dn => !dn.IsNullOrWhiteSpace(), "شماره بارنامه را وارد کنید");
            this.ValidationRule(vm => vm.SaleItemId, sii => sii > 0, "آیتم فروش باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "ماشین باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.DriverId, di => di > 0, "راننده باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.Count, c => c > 0, "مقدار تحویل داده شده باید بیشتر از صفر باشد");
            this.ValidationRule(vm => vm.DeliveryFee, df => df > 0, "هزینه تحویل باید بیشتر از صفر باشد");
        }

        public override string ToString()
        {
            return DeliveryNumber;
        }
    }
}
