using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SalaryPaymentRepository : Repository<SalaryPayment>
    {

        public override IEnumerable<SalaryPayment> All()
        {
            return _context.Set<SalaryPayment>()
                .Include(o => o.Personnel)
                .AsEnumerable();
        }

        public override SalaryPayment Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<SalaryPayment>()
                .Include(o => o.Personnel)
                .SingleOrDefault(o => o.SalaryPaymentId == intId);
        }
    }
}
