using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
