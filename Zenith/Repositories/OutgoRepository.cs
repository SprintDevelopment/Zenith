using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class OutgoRepository : Repository<Outgo>
    {

        public override IEnumerable<Outgo> All()
        {
            return _context.Set<Outgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Company)
                .AsEnumerable();
        }

        public override Outgo Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Outgo>()
                .Include(o => o.OutgoCategory)
                .Include(o => o.Company)
                .SingleOrDefault(o => o.OutgoId == intId);
        }
    }
}
