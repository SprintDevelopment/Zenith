﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Marshalling;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.UI.UserControls;
using Zenith.Assets.Values.Constants;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class BuyItem : Model
    {
        [Key]
        [Reactive]
        public long BuyItemId { get; set; }

        [Reactive]
        public int BuyId { get; set; }

        [ForeignKey(nameof(BuyId))]
        public virtual Buy Buy { get; set; }

        [Reactive]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        [Reactive]
        public virtual Material Material { get; set; }

        [Required]
        [MaxLength(LengthConstants.SMALL_STRING)]
        [Reactive]
        public string DeliveryNumber { get; set; } = string.Empty;

        [Reactive]
        public float UnitPrice { get; set; }

        [Reactive]
        public float Count { get; set; }

        [Reactive]
        public CountUnits BuyCountUnit { get; set; }

        [Reactive]
        [NotMapped]
        public UnitSelectorViewModel UnitSelectorViewModel { get; set; } = new UnitSelectorViewModel();

        [Reactive]
        [NotMapped]
        public CountSelectorViewModel CountSelectorViewModel { get; set; } = new CountSelectorViewModel();

        [NotMapped]
        [Reactive]
        public float TotalPrice { get; set; }

        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public BuyItem()
        {
            this.WhenAnyValue(m => m.UnitPrice, m => m.Count)
                .Select(x => Math.Floor(x.Item1 * x.Item2))
                .BindTo(this, m => m.TotalPrice);

            this.WhenAnyValue(m => m.BuyCountUnit)
                .BindTo(this, m => m.UnitSelectorViewModel.SelectedCountUnit);

            this.WhenAnyValue(m => m.UnitSelectorViewModel)
                .WhereNotNull()
                .Select(usvm => usvm.WhenAnyValue(vm => vm.SelectedCountUnit))
                .Switch()
                .Do(selectedUnit =>
                {
                    BuyCountUnit = selectedUnit;
                }).Subscribe();

            this.WhenAnyValue(m => m.Count)
                .BindTo(this, m => m.CountSelectorViewModel.SelectedCount);

            this.WhenAnyValue(m => m.CountSelectorViewModel)
                .WhereNotNull()
                .Select(usvm => usvm.WhenAnyValue(vm => vm.SelectedCount))
                .Switch()
                .Do(selectedCount =>
                {
                    Count = selectedCount;
                }).Subscribe();

            this.ValidationRule(vm => vm.DeliveryNumber, dn => !dn.IsNullOrWhiteSpace(), "Enter delivery number");
            this.ValidationRule(vm => vm.UnitPrice, up => up > 0, "Unit price must be greater than 0");
            this.ValidationRule(vm => vm.Count, c => c > 0, "Count must be greater than 0");
        }
    }
}
