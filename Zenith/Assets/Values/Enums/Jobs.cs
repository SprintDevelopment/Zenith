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
        [Display(Name = "")]
        DontCare = -1,
        [Display(Name = "مشخص نشده")]
        None,
        [Display(Name = "راننده")]
        Driver,
        [Display(Name = "حسابدار")]
        Accountant
    }
}
