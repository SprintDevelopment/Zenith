using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MaterialRepository : Repository<Material>
    {
        public override IEnumerable<Material> All()
        {
            return _context.Set<Material>()
                .Where(m => !m.IsMixed)
                .AsEnumerable();
        }

        public IEnumerable<Material> AllIncludeMixed()
        {
            return _context.Set<Material>()
                .AsEnumerable();
        }

        public void UpdateAmount(int materialId, float addedAmount)
        {
            var material = Single(materialId);

            if (!material.IsMixed)
            {
                material.AvailableAmount += addedAmount;
                _context.Set<Material>()
                    .Update(material);

                _context.SaveChanges();
            }
        }
    }
}
