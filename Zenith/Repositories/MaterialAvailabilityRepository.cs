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

            if(materialAvailability is null) 
            {
                materialAvailability = new MaterialAvailability
                {
                    MaterialId = materialId,
                    BuyPrice= buyPrice,
                    AvailableCount= count,
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

        public void 
    }
}
