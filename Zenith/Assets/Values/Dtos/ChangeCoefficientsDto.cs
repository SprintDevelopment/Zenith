using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Dtos
{
    public class ChangeCoefficientsDto : BaseDto
    {
        public int CompCredCoeff { get; set; }
        public int AccCredCoeff { get; set; }
        public int AccBalanceCoeff { get; set; }
        public int AccChequeBalanceCoeff { get; set; }
    }
}
