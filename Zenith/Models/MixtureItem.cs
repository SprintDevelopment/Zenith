using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    [Model(SingleName = "آیتم سازنده ترکیب", MultipleName = "آیتم های سازنده ترکیب ها")]
    public class MixtureItem : Model
    {
        [Key]
        [Reactive]
        public long MixtureItemId { get; set; }

        [Reactive]
        public int MixtureId { get; set; }

        [ForeignKey(nameof(MixtureId))]
        public Mixture Mixture { get; set; }

        [Reactive]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        [Reactive]
        public Material Material { get; set; }

        [Range(1, 99)]
        [Reactive]
        public short Percent { get; set; }

        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public MixtureItem()
        {
            this.ValidationRule(vm => vm.Percent, p => p > 0 && p <= 100, "درصد ترکیب شده باید عددی بین 1 تا 99 باشد");
        }
    }
}
