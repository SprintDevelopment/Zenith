using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;
using System.Collections.Generic;
using Zenith.Assets.Extensions;

namespace Zenith.Repositories
{
    public class BuyRepository : Repository<Buy>
    {
        BuyItemRepository BuyItemRepository = new BuyItemRepository();

        public override IEnumerable<Buy> All()
        {
            return _context.Set<Buy>()
                .Include(b => b.Company)
                .Include(b => b.Items).ThenInclude(bi => bi.Material)
                .AsEnumerable();
        }

        public override Buy Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Buy>()
                .Include(b => b.Company)
                .Include(b => b.Items).ThenInclude(bi => bi.Material)
                .SingleOrDefault(b => b.BuyId == intId);
        }

        public override Buy Add(Buy buy) 
        {
            base.Add(buy);
            BuyItemRepository.AddRange(buy.Items.Select(bi => { bi.BuyId = buy.BuyId; return bi; }));

            return buy;
        }

        public override Buy Update(Buy buy, dynamic buyId) 
        {
            base.Update(buy, buy.BuyId);

            var oldItems = BuyItemRepository.Find(bi => bi.BuyId == buy.BuyId).ToList();
            BuyItemRepository.RemoveRange(oldItems.Where(bi => !buy.Items.Any(rbi => rbi.BuyItemId == bi.BuyItemId)));

            buy.Items.ToList().ForEach(bi =>
            {
                if (bi.BuyId == 0)
                {
                    bi.BuyId = buy.BuyId;
                    BuyItemRepository.Add(bi);
                }
                else
                BuyItemRepository.Update(bi, bi.BuyItemId);
            });

            return buy;
        }
    }
}
