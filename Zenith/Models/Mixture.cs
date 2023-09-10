using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
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
    public class Mixture : Model
    {
        [Key]
        [Reactive]
        public int MixtureId { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Required]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string DisplayName { get; set; }

        [Reactive]
        public long SalePrice { get; set; }

        [Reactive]
        public CountUnits CommonSaleUnit { get; set; } = CountUnits.Meter;

        [NotMapped]
        public string SalePriceWithUnit => $"{SalePrice} ({CommonSaleUnit})";

        [Reactive]
        public int RelatedMaterialId { get; set; }

        [Reactive]
        public virtual ObservableCollection<MixtureItem> Items { get; set; } = new ObservableCollection<MixtureItem>();

        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Mixture()
        {
            var itemsObservable = this.WhenAnyValue(b => b.Items)
                .SelectMany(items => items.ToObservableChangeSet())
                .AutoRefresh(bi => bi.Percent)
                .ToCollection();
                

            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "Enter name of mixture");
            this.ValidationRule(vm => vm.DisplayName, displayName => !displayName.IsNullOrWhiteSpace(), "Enter name for display in factor");
            this.ValidationRule(vm => vm.Items, itemsObservable.Select(children => children.Count > 1 && children.Sum(i => i.Percent) == 100), "Add at least 2 materials; Sum of composition percent must be equal to 100");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
