using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    public class Delivery : TransactionModel
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
        public int SiteId { get; set; }

        [ForeignKey(nameof(SiteId))]
        [Reactive]
        public virtual Site Site { get; set; }

        [Reactive]
        public float Count { get; set; }

        [Reactive]
        public int? RelatedTaxiMachineOutgoId { get; set; }

        [Reactive]
        public int MachineId { get; set; }

        [ForeignKey(nameof(MachineId))]
        [Reactive]
        public virtual Machine Machine { get; set; }

        [NotMapped]
        [Reactive]
        public bool InfoIsRelatedToMachine { get; set; } = false;

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
        public float DeliveryFee { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Reactive]
        public bool IsIndirectDelivery { get; set; }

        [MaxLength(LengthConstants.SMALL_STRING)]
        [Reactive]
        public string? SourceDeliveryNumber { get; set; } = string.Empty;

        [MaxLength(LengthConstants.SMALL_STRING)]
        [Reactive]
        public string? LpoNumber { get; set; } = string.Empty;

        [Reactive]
        public DateTime SourceDeliveryDateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Delivery()
        {
            ShouldCloneAsNew = true;

            this.ValidationRule(vm => vm.DeliveryNumber, dn => !dn.IsNullOrWhiteSpace(), "Enter delivery number");
            this.ValidationRule(vm => vm.SaleItemId, sii => sii > 0, "Select sale item");
            this.ValidationRule(vm => vm.SiteId, si => si > 0, "Select site");
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "Select machine");
            this.ValidationRule(vm => vm.DriverId, di => di > 0, "Select driver");
            this.ValidationRule(vm => vm.Count, c => c > 0, "Delivered count must be greater than 0");
            this.ValidationRule(vm => vm.DeliveryFee, df => df > 0, "Delivery fee must be greater than 0");

            this.WhenAnyValue(m => m.IsIndirectDelivery)
                .Skip(1)
                .Do(ot =>
                {
                    if (ot)
                        this.ValidationRule(vm => vm.SourceDeliveryNumber, sdn => !sdn.IsNullOrWhiteSpace(), "Enter source delivery number");
                    else
                        this.ClearValidationRules(vm => vm.SourceDeliveryNumber);
                }).Subscribe();
        }

        public override string ToString()
        {
            return DeliveryNumber;
        }
    }
}
