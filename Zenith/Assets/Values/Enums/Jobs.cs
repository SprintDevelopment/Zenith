using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum Jobs
    {
        [Display(Name = "مشخص نشده")]
        None = 0,
        [Display(Name = "راننده")]
        Driver,
        [Display(Name = "حسابدار")]
        Accountant
    }
}
