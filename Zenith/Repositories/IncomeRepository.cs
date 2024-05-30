using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class IncomeRepository : Repository<Income>
    {
        CashRepository CashRepository = new CashRepository();

        public override IEnumerable<Income> All()
        {
            return _context.Set<Income>()
                .Include(o => o.IncomeCategory)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override async IAsyncEnumerable<Income> AllAsync()
        {
            var asyncEnumerable = _context.Set<Income>()
                .Include(o => o.IncomeCategory)
                .Include(o => o.Company)
                .OrderByDescending(s => s.DateTime)
                .AsSplitQuery()
                .AsAsyncEnumerable();

            await foreach (var item in asyncEnumerable)
            {
                yield return item;
            }
        }

        public override Income Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Income>()
                .Include(o => o.IncomeCategory)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.IncomeId == intId);
        }

        public override Income Add(Income income)
        {
            base.Add(income);

            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(income));

            return income;
        }

        public override Income Update(Income income, dynamic incomeId)
        {
            var oldIncome = Single((int)incomeId);

            base.Update(income, income.IncomeId);

            var relatedCash = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashIncome ||
                                                      c.MoneyTransactionType == MoneyTransactionTypes.NonCashIncome) && c.RelatedEntityId == income.IncomeId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .FirstOrDefault();

            if (relatedCash is not null)
            {
                MapperUtil.Mapper.Map(income, relatedCash);
                CashRepository.Update(relatedCash, relatedCash.CashId);
            }

            return income;
        }

        public override void RemoveRange(IEnumerable<Income> incomes)
        {
            var incomesIds = incomes.Select(b => b.IncomeId).ToList();

            base.RemoveRange(incomes);

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashIncome ||
                                                          c.MoneyTransactionType == MoneyTransactionTypes.NonCashIncome) && incomesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
