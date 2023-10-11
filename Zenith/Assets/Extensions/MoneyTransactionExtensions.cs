using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Extensions
{
    public static class MoneyTransactionExtensions
    {
        public static ChangeCoefficientsDto ToChangeCoefficient(this MoneyTransactionTypes moneyTransactionType)
        {
            return moneyTransactionType switch
            {
                MoneyTransactionTypes.Payment => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = -1 },
                MoneyTransactionTypes.Recieve => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashBuy => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashBuy => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.NonCashSale => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashSale => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashDelivery => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashDelivery => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashOutgo => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashOutgo => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.NonCashMachineOutgo => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashMachineOutgo => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.NonCashBuyConsumables => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashBuyConsumables => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.WorkshopSalary => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },
                MoneyTransactionTypes.TransportaionSalary => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },
                
            };
        }
    }
}
