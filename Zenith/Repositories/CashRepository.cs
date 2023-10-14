using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Zenith.Assets.Extensions;
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

            var changeCoefficients = cash.MoneyTransactionType.ToChangeCoefficient();

            if (cash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(cash.CompanyId);
                relatedCompany.CreditValue += cash.Value * changeCoefficients.CompCredCoeff;
                CompanyRepository.Update(relatedCompany, cash.CompanyId);
            }
            
            var relatedAccount = AccountRepository.Single((short)(cash.CostCenter == CostCenters.Workshop ? 1 : cash.CostCenter == CostCenters.Transportation ? 2 : 3));
            relatedAccount.Balance += cash.Value * changeCoefficients.AccBalanceCoeff;
            relatedAccount.ChequeBalance += cash.Value * changeCoefficients.AccChequeBalanceCoeff;
            relatedAccount.CreditValue += cash.Value * changeCoefficients.AccCredCoeff;

            AccountRepository.Update(relatedAccount, relatedAccount.AccountId);

            return cash;
        }

        public override Cash Update(Cash cash, dynamic cashId)
        {
            var oldCash = Single((int)cashId);
            var changeCoefficients = oldCash.MoneyTransactionType.ToChangeCoefficient();

            if (oldCash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(oldCash.CompanyId);
                relatedCompany.CreditValue -= oldCash.Value * changeCoefficients.CompCredCoeff;
                CompanyRepository.Update(relatedCompany, oldCash.CompanyId);
            }


            var relatedAccount = AccountRepository.Single((short)(cash.CostCenter == CostCenters.Workshop ? 1 : cash.CostCenter == CostCenters.Transportation ? 2 : 3));

            relatedAccount.Balance -= oldCash.Value * changeCoefficients.AccBalanceCoeff;
            relatedAccount.CreditValue -= oldCash.Value * changeCoefficients.AccCredCoeff;
            relatedAccount.ChequeBalance -= oldCash.Value * changeCoefficients.AccChequeBalanceCoeff;
            AccountRepository.Update(relatedAccount, relatedAccount.AccountId);


            base.Update(cash, cash.CashId);

            var newChangeCoefficients = cash.MoneyTransactionType.ToChangeCoefficient();

            if (cash.CompanyId.HasValue)
            {
                var relatedCompany = CompanyRepository.Single(cash.CompanyId);
                relatedCompany.CreditValue += cash.Value * newChangeCoefficients.CompCredCoeff;
                CompanyRepository.Update(relatedCompany, cash.CompanyId);
            }

            relatedAccount.CreditValue += cash.Value * newChangeCoefficients.AccCredCoeff;
            relatedAccount.Balance += cash.Value * newChangeCoefficients.AccBalanceCoeff;
            relatedAccount.ChequeBalance += cash.Value * changeCoefficients.AccChequeBalanceCoeff;
            AccountRepository.Update(relatedAccount, relatedAccount.AccountId);


            return cash;
        }

        public override void RemoveRange(IEnumerable<Cash> cashes)
        {
            cashes.Where(c => c.CompanyId.HasValue).GroupBy(c => c.CompanyId).Select(g => new
            {
                relatedCompany = CompanyRepository.Single(g.Key),
                changes = g.Sum(c => c.Value * c.MoneyTransactionType.ToChangeCoefficient().CompCredCoeff)
            }).ToList().ForEach(x =>
            {
                x.relatedCompany.CreditValue -= x.changes;
                CompanyRepository.Update(x.relatedCompany, x.relatedCompany.CompanyId);
            });

            cashes.GroupBy(c => c.CostCenter).Select(g => new
            {
                relatedAccount = AccountRepository.Single((short)(g.Key == CostCenters.Workshop ? 1 : g.Key == CostCenters.Transportation ? 2 : 3)),
                creditChanges = g.Sum(c => c.Value * c.MoneyTransactionType.ToChangeCoefficient().AccCredCoeff),
                balanceChanges = g.Sum(c => c.Value * c.MoneyTransactionType.ToChangeCoefficient().AccBalanceCoeff),
                chequeBalanceChanges = g.Sum(c => c.Value * c.MoneyTransactionType.ToChangeCoefficient().AccChequeBalanceCoeff),
            }).ToList().ForEach(x =>
            {
                x.relatedAccount.CreditValue -= x.creditChanges;
                x.relatedAccount.Balance -= x.balanceChanges;
                x.relatedAccount.ChequeBalance -= x.chequeBalanceChanges;
                AccountRepository.Update(x.relatedAccount, x.relatedAccount.AccountId);
            });

            base.RemoveRange(cashes);
        }
    }
}
