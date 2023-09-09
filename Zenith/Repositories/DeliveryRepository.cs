using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        public override Delivery Single(dynamic id)
        {
            long longId = (long)id;
            return _context.Set<Delivery>()
                .Include(d => d.Site)
                .Include(d => d.Machine)
                .Include(d => d.Driver)
                .SingleOrDefault(s => s.DeliveryId == longId);
        }

        public override IEnumerable<Delivery> Find(Expression<Func<Delivery, bool>> predicate) =>
            _context.Set<Delivery>()
                .Include(d => d.Site)
                .Include(d => d.Machine)
                .Include(d => d.Driver)
                .Where(predicate).AsEnumerable();
    }
}
