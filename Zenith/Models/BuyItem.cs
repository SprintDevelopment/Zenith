using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Marshalling;
using Zenith.Assets.Attributes;
using Zenith.Assets.UI.UserControls;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class BuyItem : Model
    {
        [Key]
        [Reactive]
        public long BuyItemId { get; set; }

        [Reactive]
        public int BuyId { get; set; }

        [ForeignKey(nameof(BuyId))]
        public virtual Buy Buy { get; set; }

        [Reactive]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        [Reactive]
        public virtual Material Material { get; set; }

        [Reactive]
        public long UnitPrice { get; set; }

        [Reactive]
        public int Count { get; set; }

        [Reactive]
        public CountUnits BuyCountUnit { get; set; }

        [Reactive]
        [NotMapped]
        public UnitSelectorViewModel UnitSelectorViewModel { get; set; } = new UnitSelectorViewModel();

        [NotMapped]
        [Reactive]
        public long TotalPrice { get; set; }

        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public BuyItem()
        {
            this.WhenAnyValue(m => m.UnitPrice, m => m.Count)
                .Select(x => x.Item1 * x.Item2)
                .BindTo(this, m => m.TotalPrice);

            this.WhenAnyValue(m => m.BuyCountUnit)
                .BindTo(this, m => m.UnitSelectorViewModel.SelectedCountUnit);

            this.WhenAnyValue(m => m.UnitSelectorViewModel)
                .WhereNotNull()
                .Select(usvm => usvm.WhenAnyValue(vm => vm.SelectedCountUnit))
                .Switch()
                .Do(selectedUnit =>
                {
                    BuyCountUnit = selectedUnit;
                }).Subscribe();

            this.ValidationRule(vm => vm.UnitPrice, up => up > 0, "Unit price must be greater than 0");
            this.ValidationRule(vm => vm.Count, c => c > 0, "Count must be greater than 0");
        }
    }
}
