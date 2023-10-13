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
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class Sale : TransactionModel
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
        public bool IsIndirectSale { get; set; }

        [Reactive]
        public short? IndirectSellerCompanyId { get; set; }

        [ForeignKey(nameof(IndirectSellerCompanyId))]
        [Reactive]
        public virtual Company? IndirectSellerCompany { get; set; }

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
        public float Price { get; set; }

        [NotMapped]
        [Reactive]
        public float DeliveryFee { get; set; }

        public Sale()
        {
            var itemsObservable = this.WhenAnyValue(s => s.Items)
                .SelectMany(items => items.ToObservableChangeSet());

            itemsObservable
                .AutoRefresh(si => si.TotalPrice)
                .ToCollection()
                .Select(items => items.Where(si => si.MixtureMaterialId is null).Sum(si => si.TotalPrice))
                .BindTo(this, m => m.Price);

            itemsObservable
                .AutoRefreshOnObservable(si => si.Deliveries.ToObservableChangeSet())
                .ToCollection()
                .Select(items => items.Sum(si => si.Deliveries.Sum(d => d.DeliveryFee)))
                .BindTo(this, m => m.DeliveryFee);

            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select the buyer company");
            this.ValidationRule(vm => vm.Items, itemsObservable.QueryWhenChanged().Select(children => children.Any()), "No material selected");

            this.WhenAnyValue(m => m.IsIndirectSale)
                .Skip(1)
                .Do(ot =>
                {
                    if (ot)
                        this.ValidationRule(vm => vm.IndirectSellerCompanyId, ci => ci > 0, "Select seller company");
                    else
                        this.ClearValidationRules(vm => vm.IndirectSellerCompanyId);
                }).Subscribe();
        }

        public override string ToString()
        {
            return $"{Company?.Name} {DateTime}";
        }
    }
}
