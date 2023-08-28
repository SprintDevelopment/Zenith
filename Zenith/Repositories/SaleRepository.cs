﻿using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;
using System.Collections.Generic;
using Zenith.Assets.Extensions;

namespace Zenith.Repositories
{
    public class SaleRepository : Repository<Sale>
    {
        SaleItemRepository SaleItemRepository = new SaleItemRepository();

        public override IEnumerable<Sale> All()
        {
            return _context.Set<Sale>()
                .Include(s => s.Company)
                .Include(s => s.Items).ThenInclude(si => si.Material)
                .AsEnumerable();
        }

        public override Sale Single(dynamic id)
        {
            int intId = (int)id;
            return _context.Set<Sale>()
                .Include(s => s.Company)
                .Include(s => s.Items).ThenInclude(si => si.Material)
                .Include(d => d.Items).ThenInclude(si => si.Deliveries).ThenInclude(d => d.Machine)
                .Include(d => d.Items).ThenInclude(si => si.Deliveries).ThenInclude(d => d.Driver)
                .SingleOrDefault(s => s.SaleId == intId);
        }

        public override Sale Add(Sale sale) 
        {
            base.Add(sale);
            SaleItemRepository.AddRange(sale.Items.Select(si => { si.SaleId = sale.SaleId; return si; }));

            return sale;
        }

        public override Sale Update(Sale sale, dynamic saleId) 
        {
            base.Update(sale, sale.SaleId);

            var oldItems = SaleItemRepository.Find(si => si.SaleId == sale.SaleId).ToList();
            SaleItemRepository.RemoveRange(oldItems.Where(si => !sale.Items.Any(rbi => rbi.SaleItemId == si.SaleItemId)));

            sale.Items.ToList().ForEach(si =>
            {
                if (si.SaleId == 0)
                {
                    si.SaleId = sale.SaleId;
                    SaleItemRepository.Add(si);
                }
                else
                SaleItemRepository.Update(si, si.SaleItemId);
            });

            return sale;
        }
    }
}
