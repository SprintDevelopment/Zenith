using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class BuyItemRepository : Repository<BuyItem>
    {
        MaterialRepository MaterialRepository = new MaterialRepository();

        public override void AddRange(IEnumerable<BuyItem> buyItems)
        {
            base.AddRange(buyItems);

            buyItems.Select(bi => new { bi.MaterialId, bi.BuyCountUnit, bi.Count })
                .ToList()
                .ForEach(m => MaterialRepository.UpdateAmount(m.MaterialId, m.Count));
        }

        public override BuyItem Update(BuyItem buyItem, dynamic buyItemId)
        {
            var preBuyItem = Single((long)buyItemId);
            MaterialRepository.UpdateAmount(preBuyItem.MaterialId, preBuyItem.Count * -1);

            base.Update(buyItem, buyItem.BuyItemId);

            MaterialRepository.UpdateAmount(buyItem.MaterialId, buyItem.Count);

            return buyItem;
        }

        public override void RemoveRange(IEnumerable<BuyItem> buyItems)
        {
            buyItems.Select(bi => new { bi.MaterialId, bi.BuyCountUnit, bi.Count })
                .ToList()
                .ForEach(m => MaterialRepository.UpdateAmount(m.MaterialId, m.Count * -1));

            //base.RemoveRange(buyItems);
        }
    }
}
