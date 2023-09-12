using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Models
{
    public class Cash : MoneyTransaction
    {
        [Key]
        [Reactive]
        public int CashId { get; set; }

    }
}
