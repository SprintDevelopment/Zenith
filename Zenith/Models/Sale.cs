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
    [Model(SingleName = "فروش", MultipleName = "فروش ها")]
    public class Sale : Model
    {
        [Key]
        [Reactive]
        public int SaleId { get; set; }

        [Reactive]
        public short CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public Company Company { get; set; }

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

        public Sale()
        {
            var itemsObservable = this.WhenAnyValue(s => s.Items)
                .SelectMany(items => items.ToObservableChangeSet());

            itemsObservable
                .AutoRefresh(si => si.TotalPrice)
                .ToCollection()
                .Select(items => items.Sum(si => si.TotalPrice))
                .BindTo(this, m => m.Price);


            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "شرکت خریدار را انتخاب کنید");
            this.ValidationRule(vm => vm.Items, itemsObservable.QueryWhenChanged().Select(children => children.Any()), "هیج محصولی برای فروش انتخاب نشده است");
        }

        public override string ToString()
        {
            return $"{Company?.Name} {DateTime}";
        }
    }
}
