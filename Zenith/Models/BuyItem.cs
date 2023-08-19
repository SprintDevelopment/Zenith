using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Marshalling;
using Zenith.Assets.Attributes;

namespace Zenith.Models
{
    [Model(SingleName = "آیتم خرید", MultipleName = "آیتم های خرید")]
    public class BuyItem : Model
    {
        [Key]
        [Reactive]
        public long BuyItemId { get; set; }

        [Reactive]
        public int BuyId { get; set; }

        [Reactive]
        public int MaterialId { get; set; }

        [Reactive]
        public Material Material { get; set; }

        [Reactive]
        public long UnitPrice { get; set; }

        [Reactive]
        public int Count { get; set; }

        [NotMapped]
        [Reactive]
        public long TotalPrice { get; set; }

        [Reactive]
        public string Comment { get; set; }

        public BuyItem()
        {
            this.WhenAnyValue(m => m.UnitPrice, m => m.Count)
                .Select(x => x.Item1 * x.Item2)
                .BindTo(this, m => m.TotalPrice);

        }
    }
}
