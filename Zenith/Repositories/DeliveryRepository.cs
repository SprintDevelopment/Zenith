using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        public override IEnumerable<Delivery> Find(Expression<Func<Delivery, bool>> predicate) => 
            _context.Set<Delivery>()
            .Include(d => d.SaleItem).ThenInclude(si => si.Material)
            .Include(d => d.SaleItem).ThenInclude(si => si.Sale).ThenInclude(s => s.Company)
            .Include(d => d.Driver)
            .Include(d => d.Machine)
            .Where(predicate).AsEnumerable();
    }
}
