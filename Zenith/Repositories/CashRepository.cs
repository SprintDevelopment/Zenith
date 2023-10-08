using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class CashRepository : Repository<Cash>
    {
        CompanyRepository CompanyRepository = new CompanyRepository();
        AccountRepository AccountRepository = new AccountRepository();

        public override IEnumerable<Cash> All()
        {
            return _context.Set<Cash>()
                .Include(s => s.Company)
                .AsEnumerable();
        }

        public override Cash Add(Cash cash)
        {
            base.Add(cash);

            var valueToAddToBalanceAndMinusFromCredit = (cash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * cash.Value;

            if (cash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(cash.CompanyId);
                relatedCompany.CreditValue += valueToAddToBalanceAndMinusFromCredit;
                CompanyRepository.Update(relatedCompany, cash.CompanyId);
            }

            var relatedAccount = AccountRepository.Single((short)(cash.CostCenter == CostCenters.Workshop ? 1 : cash.CostCenter == CostCenters.Transportation ? 2 : 3));

            if (cash.MoneyTransactionType == MoneyTransactionTypes.Direct)
            {
                relatedAccount.Balance += valueToAddToBalanceAndMinusFromCredit;
                relatedAccount.CreditValue -= valueToAddToBalanceAndMinusFromCredit;
            }
            else if (cash.MoneyTransactionType == MoneyTransactionTypes.Outgo || cash.MoneyTransactionType == MoneyTransactionTypes.WorkshopSalary || cash.MoneyTransactionType == MoneyTransactionTypes.TransportaionSalary)
            {
                relatedAccount.Balance += valueToAddToBalanceAndMinusFromCredit;
            }
            else
            {
                relatedAccount.CreditValue -= valueToAddToBalanceAndMinusFromCredit;
            }

            AccountRepository.Update(relatedAccount, relatedAccount.AccountId);

            return cash;
        }

        public override Cash Update(Cash cash, dynamic cashId)
        {
            var oldCash = Single((int)cashId);

            if (oldCash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(oldCash.CompanyId);
                relatedCompany.CreditValue -= (oldCash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * oldCash.Value;
                CompanyRepository.Update(relatedCompany, oldCash.CompanyId);
            }

            var differenceBetweenOldAndNewCash = (oldCash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * (cash.Value - oldCash.Value);

            var relatedAccount = AccountRepository.Single((short)(cash.CostCenter == CostCenters.Workshop ? 1 : cash.CostCenter == CostCenters.Transportation ? 2 : 3));


            base.Update(cash, cash.CashId);

            if (cash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(cash.CompanyId);
                relatedCompany.CreditValue += (cash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * cash.Value;
                CompanyRepository.Update(relatedCompany, cash.CompanyId);
            }

            if (cash.MoneyTransactionType == MoneyTransactionTypes.Direct)
            {
                relatedAccount.Balance += differenceBetweenOldAndNewCash;
                relatedAccount.CreditValue -= differenceBetweenOldAndNewCash;
            }
            else if (cash.MoneyTransactionType == MoneyTransactionTypes.Outgo || cash.MoneyTransactionType == MoneyTransactionTypes.WorkshopSalary || cash.MoneyTransactionType == MoneyTransactionTypes.TransportaionSalary)
            {
                relatedAccount.Balance += differenceBetweenOldAndNewCash;
            }
            else
            {
                relatedAccount.CreditValue -= differenceBetweenOldAndNewCash;
            }

            AccountRepository.Update(relatedAccount, relatedAccount.AccountId);


            return cash;
        }

        public override void RemoveRange(IEnumerable<Cash> cashes)
        {
            cashes.Where(c => c.CompanyId.HasValue).GroupBy(c => c.CompanyId).Select(g => new
            {
                relatedCompany = CompanyRepository.Single(g.Key),
                changes = g.Sum(c => (c.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * c.Value)
            }).ToList().ForEach(x =>
            {
                x.relatedCompany.CreditValue -= x.changes;
                CompanyRepository.Update(x.relatedCompany, x.relatedCompany.CompanyId);
            });

            cashes.GroupBy(c => c.CostCenter).Select(g => new
            {
                relatedAccount = AccountRepository.Single((short)(g.Key == CostCenters.Workshop ? 1 : g.Key == CostCenters.Transportation ? 2 : 3)),
                allChanges = g.Sum(c => (c.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * c.Value),
                swapChanges = g.Where(c => c.MoneyTransactionType == MoneyTransactionTypes.Direct).Sum(c => (c.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * c.Value),
                onlyPaidChanges = g.Where(c => c.MoneyTransactionType == MoneyTransactionTypes.Outgo || c.MoneyTransactionType == MoneyTransactionTypes.WorkshopSalary || c.MoneyTransactionType == MoneyTransactionTypes.TransportaionSalary)
                    .Sum(c => (c.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * c.Value)
            }).ToList().ForEach(x =>
            {
                x.relatedAccount.CreditValue += x.allChanges - x.swapChanges - x.onlyPaidChanges;
                x.relatedAccount.Balance -= x.swapChanges + x.onlyPaidChanges;
                AccountRepository.Update(x.relatedAccount, x.relatedAccount.AccountId);
            });

            base.RemoveRange(cashes);
        }
    }
}
