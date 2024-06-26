﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Extensions;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models
{
    public class OutgoBase : TransactionModel
    {
        [Key]
        [Reactive]
        public int OutgoId { get; set; }

        [Reactive]
        public short OutgoCategoryId { get; set; }

        [ForeignKey(nameof(OutgoCategoryId))]
        [Reactive]
        public virtual OutgoCategory OutgoCategory { get; set; }

        [Reactive]
        public OutgoTypes OutgoType { get; set; } = OutgoTypes.DontCare;

        [Reactive]
        public short? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company? Company { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.MEDIUM_STRING)]
        [Reactive]
        public string FactorNumber { get; set; } = string.Empty;

        [Reactive]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Reactive]
        public float Value { get; set; }

        [Reactive]
        public float Amount { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public OutgoBase()
        {
            this.ValidationRule(vm => vm.OutgoCategoryId, oci => oci > 0, "Select outgo category");
            this.ValidationRule(vm => vm.Value, value => value > 0, "Outgo value must be greater than 0");
            this.ValidationRule(vm => vm.Amount, amount => amount > 0, "Amount must be greater than 0");

            this.WhenAnyValue(m => m.OutgoType)
                .Skip(1)
                .Do(ot =>
                {
                    if (ot != OutgoTypes.UseConsumables)
                        this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select company");
                }).Subscribe();
        }

        public override string ToString()
        {
            return $"{OutgoCategory?.Title} ({DateTime})";
        }
    }
}
