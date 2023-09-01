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
        [Display(Name = "Buys")]
        Buys,
        [Display(Name = "Sales")]
        Sales,
        [Display(Name = "Companies")]
        Companies,
        [Display(Name = "Company sites")]
        Sites,
        [Display(Name = "Materials")]
        Materials,
        [Display(Name = "Mixtures")]
        Mixtures,
        [Display(Name = "Machines")]
        Machines,
        [Display(Name = "Outgoes")]
        Outgoes,
        [Display(Name = "Outgo Categories")]
        OutgoCategories,
        [Display(Name = "Personnel")]
        Personnel,
        [Display(Name = "Users")]
        Users,
        [Display(Name = "Notes")]
        Notes,
    }
}
