using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Enums
{
    public enum MoneyTransactionTypes
    {
        DontCare = -1,
        Direct,
        Buy,
        Sale,
        Delivery,
        Outgo,
        MachineOutgo,
        BuyConsumables,
        Salary,
        Cheque
    }
}
