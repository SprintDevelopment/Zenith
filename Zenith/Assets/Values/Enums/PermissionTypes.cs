using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum PermissionTypes
    {
        [Display(Name = "")]
        DontCare = -1,
        [Display(Name = "Buy")]
        Buy,
        [Display(Name = "Sale")]
        Sale,
    }
}
