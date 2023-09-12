using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;
using Zenith.Assets.Values.Constants;
using ReactiveUI.Validation.Extensions;

namespace Zenith.Models
{
    public class MoneyTransaction : Model
    {
        [Reactive]
        public TransferDirections TransferDirection { get; set; }

        [Reactive]
        public short CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Reactive]
        public virtual Company Company { get; set; }

        [Reactive]
        public long Value { get; set; }

        [Reactive]
        public DateTime IssueDateTime { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public MoneyTransaction()
        {
            this.ValidationRule(vm => vm.CompanyId, ci => ci > 0, "Select related company");
            this.ValidationRule(vm => vm.Value, v => v > 0, "Value must be greater than 0");
        }
    }
}
