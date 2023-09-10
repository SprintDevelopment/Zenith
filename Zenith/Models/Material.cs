using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Material : Model
    {
        [Key]
        [Reactive]
        public int MaterialId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public CountUnits CommonBuyUnit { get; set; } = CountUnits.Ton;

        [Reactive]
        public long BuyPrice { get; set; }

        [NotMapped]
        public string BuyPriceWithUnit => $"{BuyPrice} ({CommonBuyUnit})";

        [Reactive]
        public CountUnits CommonSaleUnit { get; set; } = CountUnits.Meter;

        [Reactive]
        public long SalePrice { get; set; }

        [NotMapped]
        public string SalePriceWithUnit => $"{SalePrice} ({CommonSaleUnit})";

        [Reactive]
        public int AvailableAmount { get; set; }

        [Reactive]
        public int MetersPerTon { get; set; }

        [Reactive]
        public bool IsMixed { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Material()
        {
            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "Enter name");
            this.ValidationRule(vm => vm.MetersPerTon, mpt => mpt > 0, "Enter meters per ton");
            ////this.ValidationRule(vm => vm.NotifyType, notifyType => notifytType > 0, "روش(های) اطلاعرسانی را انتخاب کنید");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
