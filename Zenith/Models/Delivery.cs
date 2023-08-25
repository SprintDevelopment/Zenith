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
        public SaleItem SaleItem { get; set; }

        [Reactive]
        public int Count { get; set; }

        [Reactive]
        public int MachineId { get; set; }

        [ForeignKey(nameof(MachineId))]
        [Reactive]
        public Machine Machine { get; set; }

        [Reactive]
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        [Reactive]
        public Person Driver { get; set; }

        [Required]
        [MaxLength(LengthConstants.SMALL_STRING)]
        [Reactive]
        public string BillNumber { get; set; } = string.Empty;

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Delivery()
        {
            this.ValidationRule(vm => vm.BillNumber, bn => !bn.IsNullOrWhiteSpace(), "شماره بارنامه نمی تواند خالی باشد");
            this.ValidationRule(vm => vm.SaleItemId, sii => sii > 0, "آیتم فروش باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "ماشین باید انتخاب شده باشد");
            this.ValidationRule(vm => vm.DriverId, di => di > 0, "راننده باید انتخاب شده باشد");
        }

        public override string ToString()
        {
            return BillNumber;
        }
    }
}
