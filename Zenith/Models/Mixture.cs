using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "ترکیب", MultipleName = "ترکیب ها")]
    public class Mixture : Model
    {
        [Key]
        [Reactive]
        public int MixtureId { get; set; }

        [Required(ErrorMessage = "نام ترکیب را وارد کنید")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string Name { get; set; }

        [Required(ErrorMessage = "نام نمایشی را وارد کنید")]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string DisplayName { get; set; }

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
                

            this.ValidationRule(vm => vm.Name, name => !name.IsNullOrWhiteSpace(), "نام ترکیب را وارد کنید");
            this.ValidationRule(vm => vm.DisplayName, displayName => !displayName.IsNullOrWhiteSpace(), "نام نمایشی ترکیب را وارد کنید");
            this.ValidationRule(vm => vm.Items, itemsObservable.Select(children => children.Count > 1 && children.Sum(i => i.Percent) == 100), "حداقل دو محصول جهت ترکیب انتخاب کنید؛ مجموع درصد ترکیبات باید 100 شود");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
