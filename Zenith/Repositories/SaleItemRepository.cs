using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class SaleItemRepository : Repository<SaleItem>
    {
        MaterialRepository MaterialRepository = new MaterialRepository();
        MixtureRepository MixtureRepository = new MixtureRepository();

        public override void AddRange(IEnumerable<SaleItem> saleItems)
        {
            var mixtureItemsToAdd = saleItems.Where(si => si.Material.IsMixed)
                .SelectMany(si => MixtureRepository
                    .GetItemsByRelatedMaterial(si.Material.MaterialId)
                    .Select(mi => new SaleItem
                    {
                        SaleId = si.SaleId,
                        MaterialId = mi.MaterialId,
                        UnitPrice = si.UnitPrice,
                        SaleCountUnit = si.SaleCountUnit,
                        MixtureMaterialId = si.MaterialId,
                        IsForIndirectSale = si.IsForIndirectSale,
                        Count = si.Count * mi.Percent / 100f
                    }));

            var finalItems = saleItems.Union(mixtureItemsToAdd);

            base.AddRange(finalItems);

            finalItems.Where(si => !si.IsForIndirectSale).Select(si => new { si.MaterialId, si.SaleCountUnit, si.Count })
                .ToList()
                .ForEach(m =>
                {
                    MaterialRepository.UpdateAmount(m.MaterialId, m.Count * -1);
                });
        }

        public override SaleItem Update(SaleItem saleItem, dynamic saleItemId)
        {
            var preSaleItem = Single((long)saleItemId);
            if (!preSaleItem.IsForIndirectSale)
                MaterialRepository.UpdateAmount(preSaleItem.MaterialId, preSaleItem.Count);

            base.Update(saleItem, saleItem.SaleItemId);

            if (!saleItem.IsForIndirectSale)
                MaterialRepository.UpdateAmount(saleItem.MaterialId, saleItem.Count * -1);

            var relatedMixtureItems = saleItem.Material.IsMixed ?
                MixtureRepository.GetItemsByRelatedMaterial(saleItem.MaterialId) :
                Enumerable.Empty<MixtureItem>();

            var mixedItems = _context.SaleItems
                .Where(si => si.SaleId == saleItem.SaleId && si.MixtureMaterialId == saleItem.MaterialId)
                .AsEnumerable()
                .Select(si =>
                {
                    var preCount = si.Count * (!si.IsForIndirectSale ? 1 : 0);

                    si.Count = saleItem.Count * (!saleItem.IsForIndirectSale ? 1 : 0) / 100f *
                        (relatedMixtureItems.FirstOrDefault(mi => mi.MaterialId == si.MaterialId) ?? new MixtureItem()).Percent;

                    MaterialRepository.UpdateAmount(si.MaterialId, -1 * (si.Count - preCount));

                    return si;
                })
                .ToList();

            _context.SaveChanges();

            return saleItem;
        }

        public void RemoveRangeAfterUpdate(IEnumerable<SaleItem> saleItems)
        {
            saleItems.Where(si => !si.IsForIndirectSale).Select(si => new { si.MaterialId, si.SaleCountUnit, si.Count })
                .ToList()
                .ForEach(m =>
                {
                    MaterialRepository.UpdateAmount(m.MaterialId, m.Count);
                });

            base.RemoveRange(saleItems.SelectMany(saleItem => _context.SaleItems
                .Where(si => si.SaleId == saleItem.SaleId && si.MixtureMaterialId == saleItem.MaterialId)));
            base.RemoveRange(saleItems);
        }

        public override void RemoveRange(IEnumerable<SaleItem> saleItems)
        {
            saleItems.Where(si => !si.IsForIndirectSale).GroupBy(si => si.MaterialId).Select(g => new { materialId = g.Key, count = g.Sum(si => si.Count) })
                .ToList()
                .ForEach(m =>
                {
                    MaterialRepository.UpdateAmount(m.materialId, m.count);
                });

            //base.RemoveRange(saleItems);
        }
    }
}
