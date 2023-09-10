using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MachineOutgoRepository : Repository<MachineOutgo>
    {

        public override IEnumerable<MachineOutgo> All()
        {
            return _context.Set<MachineOutgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override MachineOutgo Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<MachineOutgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Machine)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.OutgoId == intId);
        }
    }
}
