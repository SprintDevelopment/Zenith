using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Marshalling;
using Zenith.Assets.Attributes;
using Zenith.Assets.UI.UserControls;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class SaleItem : Model
    {
        [Key]
        [Reactive]
        public long SaleItemId { get; set; }

        [Reactive]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        [Reactive]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        [Reactive]
        public virtual Material Material { get; set; }

        [Reactive]
        public float UnitPrice { get; set; }

        [Reactive]
        public float Count { get; set; }

        [Reactive]
        public CountUnits SaleCountUnit { get; set; }

        [Reactive]
        [NotMapped]
        public UnitSelectorViewModel UnitSelectorViewModel { get; set; } = new UnitSelectorViewModel();

        [NotMapped]
        [Reactive]
        public float TotalPrice { get; set; }

        [Reactive]
        public string Comment { get; set; } = string.Empty;

        [Reactive]
        public virtual ObservableCollection<Delivery> Deliveries { get; set; } = new ObservableCollection<Delivery>();

        [NotMapped]
        [Reactive]
        public bool IsDeliveriesVisible { get; set; }

        public SaleItem()
        {
            this.WhenAnyValue(m => m.UnitPrice, m => m.Count)
                .Select(x => x.Item1 * x.Item2)
                .BindTo(this, m => m.TotalPrice);

            this.WhenAnyValue(m => m.SaleCountUnit)
                .BindTo(this, m => m.UnitSelectorViewModel.SelectedCountUnit);

            this.WhenAnyValue(m => m.UnitSelectorViewModel)
                .WhereNotNull()
                .Select(usvm => usvm.WhenAnyValue(vm => vm.SelectedCountUnit))
                .Switch()
                .Do(selectedUnit =>
                {
                    SaleCountUnit = selectedUnit;
                }).Subscribe();

            //this.WhenAnyValue(m => m.UnitSelectorViewModel)
            //    .WhereNotNull()
            //    .Select(usvm => usvm.WhenAnyValue(vm => vm.SelectedCountUnit).Where(vm => UnitSelectorViewModel.IsLocked))
            //    .Switch()
            //    .Do(selectedUnit =>
            //    {
            //        UnitPrice = selectedUnit == CountUnits.Ton ?
            //        UnitPrice * Material.MetersPerTon :
            //        UnitPrice / Material.MetersPerTon;

            //        SaleCountUnit = selectedUnit;
            //    }).Subscribe();

            this.ValidationRule(vm => vm.UnitPrice, up => up > 0, "Unit price must be greater than 0");
            this.ValidationRule(vm => vm.Count, c => c > 0, "Sold count must be greater than 0");
        }
    }
}
