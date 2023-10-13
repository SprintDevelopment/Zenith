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

                MoneyTransactionTypes.NonCashIndirectBuy => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.NonCashIndirectSale => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashIndirectSale => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashDelivery => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashDelivery => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashOutgo => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashOutgo => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.NonCashMachineOutgo => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashMachineOutgo => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.NonCashIncome => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashIncome => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashMachineIncome => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashMachineIncome => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = +1 },

                MoneyTransactionTypes.NonCashBuyConsumables => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0 },
                MoneyTransactionTypes.CashBuyConsumables => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },

                MoneyTransactionTypes.WorkshopSalary => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },
                MoneyTransactionTypes.TransportaionSalary => new ChangeCoefficientsDto { CompCredCoeff = 0, AccCredCoeff = 0, AccBalanceCoeff = -1 },
                
                MoneyTransactionTypes.NotPassedPaidCheque => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = 0, AccChequeBalanceCoeff = -1 },
                MoneyTransactionTypes.NotPassedRecievedCheque => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = 0, AccChequeBalanceCoeff = +1 },
                
                MoneyTransactionTypes.PassedPaidCheque => new ChangeCoefficientsDto { CompCredCoeff = -1, AccCredCoeff = +1, AccBalanceCoeff = -1, AccChequeBalanceCoeff = 0 },
                MoneyTransactionTypes.PassedRecievedCheque => new ChangeCoefficientsDto { CompCredCoeff = +1, AccCredCoeff = -1, AccBalanceCoeff = +1, AccChequeBalanceCoeff = 0 },
                
            };
        }
    }
}
