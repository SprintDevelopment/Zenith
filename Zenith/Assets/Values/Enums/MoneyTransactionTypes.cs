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
        NonCashBuy,
        CashBuy,
        NonCashSale,
        CashSale,
        NonCashIndirectBuy,
        NonCashIndirectSale,
        CashIndirectSale,
        NonCashDelivery,
        CashDelivery,
        NonCashOutgo,
        CashOutgo,
        NonCashMachineOutgo,
        CashMachineOutgo,
        NonCashBuyConsumables,
        CashBuyConsumables,
        NonCashIncome,
        CashIncome,
        NonCashMachineIncome,
        CashMachineIncome,
        WorkshopSalary,
        TransportaionSalary,
        NotPassedRecievedCheque,
        NotPassedPaidCheque,
        PassedRecievedCheque,
        PassedPaidCheque
    }
}
