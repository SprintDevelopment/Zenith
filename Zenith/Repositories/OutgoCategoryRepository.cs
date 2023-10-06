using Zenith.Models;

namespace Zenith.Repositories
{
    public class OutgoCategoryRepository : Repository<OutgoCategory>
    {
        public void UpdateAmount(short outgoCategoryId, float addedAmount, float addedPrice)
        {
            var category = Single(outgoCategoryId);
            category.ApproxPriceForEach = (category.ReservedAmount + addedAmount) == 0 ? 0 : (category.ApproxPriceForEach * category.ReservedAmount + addedPrice) / (category.ReservedAmount + addedAmount);
            category.ReservedAmount += addedAmount;
            _context.Set<OutgoCategory>().Update(category);

            _context.SaveChanges();
        }
    }
}
