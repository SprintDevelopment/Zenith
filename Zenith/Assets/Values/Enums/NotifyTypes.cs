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
        [Display(Name = "نیازی به یادآوری نیست")]
        NoNeedToNotify = 0,
        [Display(Name = "یادآوری در پایین پنجره")]
        FooterNotify
    }
}
