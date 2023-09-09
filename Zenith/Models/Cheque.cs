using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Constants;

namespace Zenith.Models
{
    public class Cheque : Model
    {
        [Key]
        [Reactive]
        public int ChequeId { get; set; }

        [Reactive]
        public int SaleId { get; set; }

        [Reactive]
        [ForeignKey(nameof(SaleId))]
        public Sale Sale { get; set; }

        [Reactive]
        public string ChequeNumber { get; set; }

        [Reactive]
        public long ChequeValue { get; set; }

        [Reactive]
        public DateTime IssueDateTime { get; set; }

        [Reactive]
        public DateTime ChequeDate { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(LengthConstants.VERY_LARGE_STRING)]
        [Reactive]
        public string Comment { get; set; } = string.Empty;

        public Cheque()
        {
            this.ValidationRule(vm => vm.ChequeNumber, cn => !cn.IsNullOrWhiteSpace(), "Enter cheque number");
            this.ValidationRule(vm => vm.ChequeValue, cv => cv > 0, "Cheque value must be greater than 0");

        }
    }
}
