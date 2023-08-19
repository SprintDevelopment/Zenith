using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;

namespace Zenith.Repositories
{
    public class BuyRepository : Repository<Buy>
    {
        public override Buy Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Buy>()
            .Include(b => b.Company)
            .Include(b => b.Items)
            .SingleOrDefault(b => b.BuyId == intId);
        }

        public override Buy Add(Buy buy) 
        {
            var buyWithoutItems = MapperUtil.Mapper.Map<Buy>(buy);
            buyWithoutItems.Items.Select(bi => bi.Material = null).ToList();

            base.Add(buyWithoutItems);
            SaveChanges();

            //_context.BuyItems.AddRange(buy.Items.Select(bi => bi.BuyId = buy.BuyId));
            //SaveChanges();

            return buy;
        }
    }
}
