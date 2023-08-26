﻿using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;
using System.Collections.Generic;
using Zenith.Assets.Extensions;

namespace Zenith.Repositories
{
    public class MixtureRepository : Repository<Mixture>
    {
        MixtureItemRepository MixtureItemRepository = new MixtureItemRepository();

        public override IEnumerable<Mixture> All()
        {
            return _context.Set<Mixture>()
                .Include(b => b.Items).ThenInclude(bi => bi.Material)
                .AsEnumerable();
        }

        public override Mixture Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Mixture>()
                .Include(b => b.Items).ThenInclude(bi => bi.Material)
                .SingleOrDefault(b => b.MixtureId == intId);
        }

        public override Mixture Add(Mixture mixture) 
        {
            base.Add(mixture);
            MixtureItemRepository.AddRange(mixture.Items.Select(bi => { bi.MixtureId = mixture.MixtureId; return bi; }));

            return mixture;
        }

        public override Mixture Update(Mixture mixture, dynamic mixtureId) 
        {
            base.Update(mixture, mixture.MixtureId);

            var oldItems = MixtureItemRepository.Find(bi => bi.MixtureId == mixture.MixtureId).ToList();
            MixtureItemRepository.RemoveRange(oldItems.Where(bi => !mixture.Items.Any(rbi => rbi.MixtureItemId == bi.MixtureItemId)));

            mixture.Items.ToList().ForEach(bi =>
            {
                if (bi.MixtureId == 0)
                {
                    bi.MixtureId = mixture.MixtureId;
                    MixtureItemRepository.Add(bi);
                }
                else
                MixtureItemRepository.Update(bi, bi.MixtureItemId);
            });

            return mixture;
        }
    }
}