using System.Linq;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MaterialAvailabilityRepository : Repository<MaterialAvailability>
    {
        // This method will call when a buy created, updated or removed
        public void AddOrUpdate(int materialId, float buyPrice, float count)
        {
            var materialAvailability = _context.Set<MaterialAvailability>()
                .FirstOrDefault(ma => ma.MaterialId == materialId && ma.BuyPrice == buyPrice);

            if (materialAvailability is null)
            {
                materialAvailability = new MaterialAvailability
                {
                    MaterialId = materialId,
                    BuyPrice = buyPrice,
                    AvailableCount = count,
                    TotalBoughtCount = count
                };

                base.Add(materialAvailability);
            }
            else
            {
                materialAvailability.AvailableCount += count;
                materialAvailability.TotalBoughtCount += count;

                base.Update(materialAvailability, materialAvailability.MaterialAvailabilityId);
            }
        }

        public void UpdateCountOnly(int materialId, float count)
        {
            if (count > 0)
            {
                //var sumOfAchievedCount = 0f;

                var itemsToUpdateCount = _context.Set<MaterialAvailability>()
                    .Where(ma => ma.MaterialId == materialId && ma.AvailableCount != 0)
                    .AsEnumerable()
                    .TakeWhile(ma => count > 0)
                    .Select(ma => { count -= ma.AvailableCount; ma.AvailableCount = 0; return ma; });

                itemsToUpdateCount.Last()
                    .AvailableCount = count * -1;
            }
            else
            {
                var itemsToUpdateCount = _context.Set<MaterialAvailability>()
                    .Where(ma => ma.MaterialId == materialId && ma.TotalBoughtCount > ma.AvailableCount)
                    .AsEnumerable()
                    .OrderByDescending(ma => ma.MaterialAvailabilityId)
                    .TakeWhile(ma => count < 0)
                    .Select(ma => { count += ma.TotalBoughtCount - ma.AvailableCount; ma.AvailableCount = ma.TotalBoughtCount; return ma; });

                itemsToUpdateCount.Last()
                    .AvailableCount -= count * 1;
            }

            _context.SaveChanges();
        }
    }
}
