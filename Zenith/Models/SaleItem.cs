using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Marshalling;
using Zenith.Assets.Attributes;

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
        public long UnitPrice { get; set; }

        [Reactive]
        public int Count { get; set; }

        [NotMapped]
        [Reactive]
        public long TotalPrice { get; set; }

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

            this.ValidationRule(vm => vm.UnitPrice, up => up > 0, "Unit price must be greater than 0");
            this.ValidationRule(vm => vm.Count, c => c > 0, "Sold count must be greater than 0");
        }
    }
}
