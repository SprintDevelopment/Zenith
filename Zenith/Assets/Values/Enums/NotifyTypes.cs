using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum NotifyTypes
    {
        [Display(Name = "")]
        DontCare = -1,
        [Display(Name = "No need to remind")]
        NoNeedToNotify,
        [Display(Name = "Show at bottom")]
        FooterNotify
    }
}
