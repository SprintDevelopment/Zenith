using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    [Flags]
    public enum CompanyTypes
    {
        DontCare = -1,
        Buyer = 1,
        Seller = 2,
        Both = 4,
        Other = 8
    }
}
