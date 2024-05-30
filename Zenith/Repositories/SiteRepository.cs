using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SiteRepository : Repository<Site>
    {
        //public override IEnumerable<Site> All()
        //{
        //    return _context.Set<Site>()
        //        .Include(s => s.Company)
        //        .AsEnumerable();
        //}

        public override async IAsyncEnumerable<Site> AllAsync()
        {
            var asyncEnumerable = _context.Set<Site>()
                .Include(s => s.Company)
                .AsSplitQuery()
                .AsAsyncEnumerable();

            await foreach (var item in asyncEnumerable)
            {
                yield return item;
            }
        }

        public override Site Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Site>()
                .Include(s => s.Company)
                .SingleOrDefault(s => s.SiteId == intId);
        }

        public override IEnumerable<Site> FindForSearch(Expression<Func<Site, bool>> predicate)
        {
            var sites = base.FindForSearch(predicate);
            sites.FirstOrDefault().Name = "All";

            return sites;
        }
    }
}
