using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SiteRepository : Repository<Site>
    {

        public override IEnumerable<Site> All()
        {
            return _context.Set<Site>()
                .Include(s => s.Company)
                .AsEnumerable();
        }

        public override Site Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Site>()
                .Include(s => s.Company)
                .SingleOrDefault(s => s.SiteId == intId);
        }
    }
}
