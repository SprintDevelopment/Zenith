using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class CashRepository : Repository<Cash>
    {
        CompanyRepository CompanyRepository = new CompanyRepository();

        public override IEnumerable<Cash> All()
        {
            return _context.Set<Cash>()
                .Include(s => s.Company)
                .AsEnumerable();
        }

        public override Cash Add(Cash cash)
        {
            base.Add(cash);

            var relatedCompany = CompanyRepository.Single(cash.CompanyId);
            relatedCompany.CreditValue += (cash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * cash.Value;
            CompanyRepository.Update(relatedCompany, cash.CompanyId);

            return cash;
        }

        public override Cash Update(Cash cash, dynamic cashId)
        {
            var oldCash = Single(cashId);

            var relatedCompany = CompanyRepository.Single(oldCash.CompanyId);
            relatedCompany.CreditValue -= (oldCash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * oldCash.Value;
            CompanyRepository.Update(relatedCompany, oldCash.CompanyId);

            base.Update(cash, cash.CashId);

            relatedCompany = CompanyRepository.Single(cash.CompanyId);
            relatedCompany.CreditValue += (cash.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * cash.Value;
            CompanyRepository.Update(relatedCompany, cash.CompanyId);

            return cash;
        }

        public override void RemoveRange(IEnumerable<Cash> cashes)
        {
            cashes.GroupBy(c => c.CompanyId).Select(g => new
            {
                relatedCompany = CompanyRepository.Single(g.Key),
                changes = g.Sum(c => (c.TransferDirection == TransferDirections.FromCompnay ? +1 : -1) * c.Value)
            }).ToList().ForEach(x => 
            {
                x.relatedCompany.CreditValue -= x.changes;
                CompanyRepository.Update(x.relatedCompany, x.relatedCompany.CompanyId);
            });

            base.RemoveRange(cashes);
        }
    }
}
