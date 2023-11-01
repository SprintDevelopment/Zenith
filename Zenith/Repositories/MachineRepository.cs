using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MachineRepository : Repository<Machine>
    {
        public override IEnumerable<Machine> All()
        {
            return _context.Set<Machine>()
                .Include(s => s.OwnerCompany)
                .AsEnumerable();
        }

        public override Machine Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Machine>()
                .Include(s => s.OwnerCompany)
                .SingleOrDefault(s => s.MachineId == intId);
        }
    }
}
