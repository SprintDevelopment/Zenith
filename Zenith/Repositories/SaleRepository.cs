using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;
using System.Collections.Generic;
using Zenith.Assets.Values.Enums;
using AutoMapper;

namespace Zenith.Repositories
{
    public class SaleRepository : Repository<Sale>
    {
        SaleItemRepository SaleItemRepository = new SaleItemRepository();
        CashRepository CashRepository = new CashRepository();

        public override IEnumerable<Sale> All()
        {
            return _context.Set<Sale>()
                .Include(s => s.Company)
                .Include(s => s.Items).ThenInclude(si => si.Material)
                .Include(s => s.Items).ThenInclude(si => si.Deliveries)
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
                .Include(d => d.Items).ThenInclude(si => si.Deliveries).ThenInclude(d => d.Site)
                .SingleOrDefault(s => s.SaleId == intId);
        }

        public override Sale Add(Sale sale)
        {
            base.Add(sale);
            SaleItemRepository.AddRange(sale.Items.Select(si => { si.SaleId = sale.SaleId; return si; }));

            var cashForWorkshop = new Cash 
            {
                CostCenter = CostCenters.Workshop,
                Value = sale.Items.Sum(si => si.TotalPrice)
            };

            var cashForTransportation = new Cash 
            {
                CostCenter = CostCenters.Transportation,
                Value = sale.Items.Sum(si => si.Deliveries.Sum(d => d.DeliveryFee))
            };

            MapperUtil.Mapper.Map(sale, cashForWorkshop);
            MapperUtil.Mapper.Map(sale, cashForTransportation);

            CashRepository.Add(cashForWorkshop);
            CashRepository.Add(cashForTransportation);

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

            var relatedCashes = CashRepository
                .Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Sale && c.RelatedEntityId == sale.SaleId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .Take(2);
            
            var workshopCash = relatedCashes.FirstOrDefault(c => c.CostCenter == CostCenters.Workshop);
            if (workshopCash is not null)
            {
                workshopCash.Value = sale.Items.Sum(si => si.TotalPrice);
                workshopCash.IssueDateTime = sale.DateTime;

                CashRepository.Update(workshopCash, workshopCash.CashId);
            }

            var transportationCash = relatedCashes.FirstOrDefault(c => c.CostCenter == CostCenters.Transportation);
            if (transportationCash is not null)
            {
                transportationCash.Value = sale.Items.Sum(si => si.Deliveries.Sum(d => d.DeliveryFee));
                transportationCash.IssueDateTime = sale.DateTime;

                CashRepository.Update(transportationCash, transportationCash.CashId);
            }
            
            return sale;
        }

        public override void RemoveRange(IEnumerable<Sale> sales)
        {
            var salesIds = sales.Select(s => s.SaleId).ToList();

            // Integrity for materials available amount
            var items = SaleItemRepository.Find(si => salesIds.Contains(si.SaleId)).ToList();
            SaleItemRepository.RemoveRange(items);

            base.RemoveRange(sales);

            var relatedCashes = CashRepository.Find(c => c.MoneyTransactionType == MoneyTransactionTypes.Sale && salesIds.Contains(c.RelatedEntityId)).Take(2);
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
