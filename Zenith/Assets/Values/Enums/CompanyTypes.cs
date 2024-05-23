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
        RelatedToSale = 1,
        RelatedToBuy = 2,
        RelatedToOutgo = 4,
        RelatedToIncome= 8
    }
}
