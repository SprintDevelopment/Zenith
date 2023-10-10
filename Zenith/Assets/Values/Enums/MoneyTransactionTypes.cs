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
        Recieve,
        Payment,
        NonCahBuy,
        CahBuy,
        NonCashSale,
        CashSale,
        NonCashDelivery,
        CashDelivery,
        NonCashOutgo,
        CashOutgo,
        NonCashMachineOutgo,
        CashMachineOutgo,
        NonCashBuyConsumables,
        CashBuyConsumables,
        WorkshopSalary,
        TransportaionSalary,
        Cheque
    }
}
