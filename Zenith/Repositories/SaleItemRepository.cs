using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SaleItemRepository : Repository<SaleItem>
    {
        MaterialRepository MaterialRepository = new MaterialRepository();
        MaterialAvailabilityRepository MaterialAvailabilityRepository = new MaterialAvailabilityRepository();

        public override void AddRange(IEnumerable<SaleItem> saleItems)
        {
            base.AddRange(saleItems);

            saleItems.Select(si => new { si.MaterialId, si.SaleCountUnit, si.Count })
                .ToList()
                .ForEach(m =>
                {
                    MaterialRepository.UpdateAmount(m.MaterialId, m.Count * -1);
                    MaterialAvailabilityRepository.UpdateCountOnly(m.MaterialId, m.Count);
                });
        }

        public override SaleItem Update(SaleItem saleItem, dynamic saleItemId)
        {
            var preSaleItem = Single((long)saleItemId);
            MaterialRepository.UpdateAmount(preSaleItem.MaterialId, preSaleItem.Count);
            MaterialAvailabilityRepository.UpdateCountOnly(preSaleItem.MaterialId, preSaleItem.Count * -1);

            base.Update(saleItem, saleItem.SaleItemId);
            
            MaterialRepository.UpdateAmount(saleItem.MaterialId, saleItem.Count * -1);
            MaterialAvailabilityRepository.UpdateCountOnly(saleItem.MaterialId, saleItem.Count);

            return saleItem;
        }

        public override void RemoveRange(IEnumerable<SaleItem> saleItems)
        {
            saleItems.Select(si => new { si.MaterialId, si.SaleCountUnit, si.Count })
                .ToList()
                .ForEach(m =>
                {
                    MaterialRepository.UpdateAmount(m.MaterialId, m.Count);
                    MaterialAvailabilityRepository.UpdateCountOnly(m.MaterialId, m.Count * -1);
                });

            //base.RemoveRange(saleItems);
        }
    }
}
