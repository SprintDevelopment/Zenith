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
        [Display(Name = "Not specified")]
        None,
        [Display(Name = "Driver")]
        Driver,
        [Display(Name = "Accountant")]
        Accountant
    }
}
