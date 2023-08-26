using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum CompanyTypes
    {
        [Display(Name = "")]
        DontCare = -1,
        [Display(Name = "خریدار")]
        Buyer,
        [Display(Name = "فروشنده")]
        Seller,
        [Display(Name = "هم خریدار و هم فروشنده")]
        Both
    }
}
