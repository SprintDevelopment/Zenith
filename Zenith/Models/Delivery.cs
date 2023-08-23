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
using Zenith.Migrations;
using System.Windows.Media.Media3D;

namespace Zenith.Models
{
    [Model(SingleName = "", MultipleName = "سایت ها")]
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
