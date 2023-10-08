using Zenith.Models;

namespace Zenith.Repositories
{
    public class OutgoCategoryRepository : Repository<OutgoCategory>
    {
        public void UpdateAmount(short outgoCategoryId, float addedAmount, float addedPrice)
        {
            var category = Single(outgoCategoryId);
            category.ApproxUnitPrice = (category.ReservedAmount + addedAmount) == 0 ? 0 : (category.ApproxUnitPrice * category.ReservedAmount + addedPrice) / (category.ReservedAmount + addedAmount);
            category.ReservedAmount += addedAmount;
            _context.Set<OutgoCategory>().Update(category);

            _context.SaveChanges();
        }
    }
}
