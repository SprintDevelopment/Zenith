using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MachineIncomeRepository : Repository<MachineIncome>
    {
        CashRepository CashRepository = new CashRepository();

        public override IEnumerable<MachineIncome> All()
        {
            return _context.Set<MachineIncome>()
                .Include(o => o.IncomeCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override MachineIncome Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<MachineIncome>()
                .Include(o => o.IncomeCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.IncomeId == intId);
        }

        public override MachineIncome Add(MachineIncome machineIncome)
        {
            base.Add(machineIncome);

            CashRepository.Add(MapperUtil.Mapper.Map<Cash>(machineIncome));

            return machineIncome;
        }

        public override MachineIncome Update(MachineIncome machineIncome, dynamic machineIncomeId)
        {
            var oldMachineIncome = Single((int)machineIncomeId);

            base.Update(machineIncome, machineIncome.IncomeId);

            var relatedCash = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashMachineIncome || c.MoneyTransactionType == MoneyTransactionTypes.NonCashMachineIncome) && c.RelatedEntityId == machineIncome.IncomeId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .FirstOrDefault();

            if (relatedCash is not null)
            {
                MapperUtil.Mapper.Map(machineIncome, relatedCash);
                CashRepository.Update(relatedCash, relatedCash.CashId);
            }

            return machineIncome;
        }

        public override void RemoveRange(IEnumerable<MachineIncome> machineIncomes)
        {
            var machineIncomesIds = machineIncomes.Select(b => b.IncomeId).ToList();

            base.RemoveRange(machineIncomes);

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashMachineIncome ||
                                                    c.MoneyTransactionType == MoneyTransactionTypes.NonCashMachineIncome) && machineIncomesIds.Contains(c.RelatedEntityId));
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
