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
            //var mixtureItems = new List<MixtureItem>() 
            //{ 
            //    new MixtureItem { Material = material, Percent = 100 } 
            //};

            //if (material.IsMixed)
            //{
            //    mixtureItems = _context.Set<Mixture>()
            //        .Include(m => m.Items).ThenInclude(mi => mi.Material)
            //        .FirstOrDefault(m => m.RelatedMaterialId == materialId).Items.ToList();
            //}

            //mixtureItems.ForEach(m =>
            //{
            //    m.Material.AvailableAmount += addedAmount * m.Percent / 100;
            //});

            //_context.Set<Material>()
            //    .UpdateRange(mixtureItems.Select(mi => mi.Material));

            material.AvailableAmount += addedAmount;
            _context.Set<Material>()
                .Update(material);

            _context.SaveChanges();
        }
    }
}
