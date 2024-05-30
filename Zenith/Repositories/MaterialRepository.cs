﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.Repositories
{
    public class MaterialRepository : Repository<Material>
    {
        //public override IEnumerable<Material> All()
        //{
        //    return _context.Set<Material>()
        //        .Where(m => !m.IsMixed)
        //        .AsEnumerable();
        //}

        public override async IAsyncEnumerable<Material> AllAsync()
        {
            var asyncEnumerable = _context.Set<Material>()
                .Where(m => !m.IsMixed)
                .AsAsyncEnumerable();

            await foreach (var item in asyncEnumerable)
            {
                yield return item;
            }
        }

        public IEnumerable<Material> AllIncludeMixed()
        {
            return _context.Set<Material>()
                .AsEnumerable();
        }

        public void UpdateAmount(int materialId, float addedAmount, CountUnits countUnits)
        {
            var material = Single(materialId);

            if (!material.IsMixed)
            {
                material.AvailableAmount += addedAmount * countUnits.ToInt();
                _context.Set<Material>()
                    .Update(material);

                _context.SaveChanges();
            }
        }
    }
}
