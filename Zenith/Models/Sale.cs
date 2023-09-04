using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reactive.Linq;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    public class Sale : Model
    {
        [Key]
        [Reactive]
        public int SaleId { get; set; }

        [Reactive]
        public short CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company Company { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        [Reactive]
        public virtual ObservableCollection<SaleItem> Items { get; set; } = new ObservableCollection<SaleItem>();

        [NotMapped]
        [Reactive]
        public long Price { get; set; }

        [NotMapped]
        [Reactive]
        public long DeliveryFee { get; set; }

        public Sale()
        {
            var itemsObservable = this.WhenAnyValue(s => s.Items)
                .SelectMany(items => items.ToObservableChangeSet());

            itemsObservable
                .AutoRefresh(si => si.TotalPrice)
                .ToCollection()
                .Select(items => items.Sum(si => si.TotalPrice))
                .BindTo(this, m => m.Price);

            itemsObservable
                .AutoRefreshOnObservable(si => si.Deliveries.ToObservableChangeSet())
                .ToCollection()
                .Select(items => items.Sum(si => si.Deliveries.Sum(d => d.DeliveryFee)))
                .BindTo(this, m => m.DeliveryFee);

            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select the buyer company");
            this.ValidationRule(vm => vm.Items, itemsObservable.QueryWhenChanged().Select(children => children.Any()), "No material selected");
        }

        public override string ToString()
        {
            return $"{Company?.Name} {DateTime}";
        }
    }
}
