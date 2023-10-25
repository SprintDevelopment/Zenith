using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum BackupIntervals
    {
        DontCare = -1,
        Never = 0,
        OneHour = 1,
        TwoHour = 2,
        FourHour = 4,
        EightHour = 8,
    }
}
