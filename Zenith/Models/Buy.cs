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
    [Model(SingleName = "خرید", MultipleName = "خرید ها")]
    public class Buy : Model
    {
        [Key]
        [Reactive]
        public int BuyId { get; set; }

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
        public virtual ObservableCollection<BuyItem> Items { get; set; } = new ObservableCollection<BuyItem>();

        public Buy()
        {
            var haveAnyItem = this.WhenAnyValue(b => b.Items)
                .SelectMany(items => items.ToObservableChangeSet().QueryWhenChanged())
                .Select(children => children.Any());

            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "شرکت فروشنده را انتخاب کنید");
            this.ValidationRule(vm => vm.Items, haveAnyItem, "هیج محصولی برای خرید انتخاب نشده است");
        }

        public override string ToString()
        {
            return $"{Company?.Name} {DateTime}";
        }
    }
}
