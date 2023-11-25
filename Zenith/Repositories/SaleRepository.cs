using Zenith.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Zenith.Assets.Utils;
using System.Collections.Generic;
using Zenith.Assets.Values.Enums;
using AutoMapper;
using Zenith.Assets.Extensions;

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

        public IEnumerable<Sale> FindByLpo(string lpoNumber)
        {
            return _context.Set<Sale>()
                .Include(s => s.Company)
                .Include(s => s.Items).ThenInclude(si => si.Material)
                .Include(s => s.Items).ThenInclude(si => si.Deliveries)
                .Where(s => s.Items.Any(i => i.Deliveries.Any(d => d.LpoNumber == lpoNumber)))
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
            SaleItemRepository.AddRange(sale.Items.Select(si => { si.SaleId = sale.SaleId; si.IsForIndirectSale = sale.IsIndirectSale; return si; }));

            var cashForWorkshop = new Cash
            {
                CostCenter = CostCenters.Workshop,
                MoneyTransactionType = sale.CashState == CashStates.NonCash ?
                    (sale.IsIndirectSale ? MoneyTransactionTypes.NonCashIndirectSale : MoneyTransactionTypes.NonCashSale) :
                    (sale.IsIndirectSale ? MoneyTransactionTypes.CashIndirectSale : MoneyTransactionTypes.CashSale),
                Value = sale.Items.Sum(si => si.TotalPrice)
            };

            var cashForTransportation = new Cash
            {
                CostCenter = CostCenters.Transportation,
                MoneyTransactionType = sale.CashState == CashStates.NonCash ? MoneyTransactionTypes.NonCashDelivery : MoneyTransactionTypes.CashDelivery,
                Value = sale.Items.Sum(si => si.Deliveries.Sum(d => d.DeliveryFee))
            };

            MapperUtil.Mapper.Map(sale, cashForWorkshop);
            MapperUtil.Mapper.Map(sale, cashForTransportation);

            CashRepository.Add(cashForWorkshop);
            CashRepository.Add(cashForTransportation);

            if (sale.IsIndirectSale)
            {
                var cashForIndirectBuy = new Cash
                {
                    CostCenter = CostCenters.Workshop,
                    MoneyTransactionType = MoneyTransactionTypes.NonCashIndirectBuy,
                    Value = sale.Items.Sum(si => si.TotalPrice)
                };

                MapperUtil.Mapper.Map(sale, cashForIndirectBuy);
                cashForIndirectBuy.CompanyId = sale.IndirectSellerCompanyId;

                CashRepository.Add(cashForIndirectBuy);
            }

            return sale;
        }

        public override Sale Update(Sale sale, dynamic saleId)
        {
            base.Update(sale, sale.SaleId);

            var oldItems = SaleItemRepository.Find(si => si.SaleId == sale.SaleId).ToList();
            SaleItemRepository.RemoveRangeAfterUpdate(
                oldItems.Where(si => si.MixtureMaterialId is null && !sale.Items.Any(rbi => rbi.SaleItemId == si.SaleItemId))
                .Union(
                    oldItems.Where(si => si.MixtureMaterialId is not null && !sale.Items.Any(rbi => rbi.MixtureMaterialId == si.MixtureMaterialId))));

            sale.Items.Where(si => si.MixtureMaterialId is null).ToList().ForEach(si =>
            {
                si.IsForIndirectSale = sale.IsIndirectSale;

                if (si.SaleId == 0)
                {
                    si.SaleId = sale.SaleId;
                    SaleItemRepository.Add(si);
                }
                else
                    SaleItemRepository.Update(si, si.SaleItemId);
            });

            var relatedCashes = CashRepository
                .Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashSale ||
                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashSale ||
                            c.MoneyTransactionType == MoneyTransactionTypes.CashIndirectSale ||
                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashIndirectSale ||
                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashIndirectBuy ||
                            c.MoneyTransactionType == MoneyTransactionTypes.CashDelivery ||
                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashDelivery) && c.RelatedEntityId == sale.SaleId)
                .Select(c => MapperUtil.Mapper.Map<Cash>(c))
                .Take(3);

            var workshopCash = relatedCashes
                .FirstOrDefault(c => c.CostCenter == CostCenters.Workshop && c.MoneyTransactionType != MoneyTransactionTypes.NonCashIndirectBuy);
            if (workshopCash is not null)
            {
                workshopCash.MoneyTransactionType = sale.CashState == CashStates.NonCash ?
                    (sale.IsIndirectSale ? MoneyTransactionTypes.NonCashIndirectSale : MoneyTransactionTypes.NonCashSale) :
                    (sale.IsIndirectSale ? MoneyTransactionTypes.CashIndirectSale : MoneyTransactionTypes.CashSale);
                workshopCash.Value = sale.Items.Where(si => si.MixtureMaterialId is null).Sum(si => si.TotalPrice);
                workshopCash.IssueDateTime = sale.DateTime;

                CashRepository.Update(workshopCash, workshopCash.CashId);
            }

            var cashForIndirectBuy = relatedCashes
                .FirstOrDefault(c => c.CostCenter == CostCenters.Workshop && c.MoneyTransactionType == MoneyTransactionTypes.NonCashIndirectBuy);
            if(cashForIndirectBuy is not null && sale.IsIndirectSale)
            {
                cashForIndirectBuy.Value = sale.Items.Where(si => si.MixtureMaterialId is null).Sum(si => si.TotalPrice);
                cashForIndirectBuy.IssueDateTime = sale.DateTime;
                cashForIndirectBuy.CompanyId = sale.IndirectSellerCompanyId;

                CashRepository.Update(cashForIndirectBuy, cashForIndirectBuy.CashId);
            }
            else if(sale.IsIndirectSale)
            {
                cashForIndirectBuy = new Cash
                {
                    CostCenter = CostCenters.Workshop,
                    MoneyTransactionType = MoneyTransactionTypes.NonCashIndirectBuy,
                    Value = sale.Items.Sum(si => si.TotalPrice)
                };

                MapperUtil.Mapper.Map(sale, cashForIndirectBuy);
                cashForIndirectBuy.CompanyId = sale.IndirectSellerCompanyId;

                CashRepository.Add(cashForIndirectBuy);
            }
            else if(cashForIndirectBuy is not null)
            {
                cashForIndirectBuy.CompanyId = sale.IndirectSellerCompanyId;
                CashRepository.RemoveRange(Enumerable.Empty<Cash>().Append(cashForIndirectBuy));
            }

            var transportationCash = relatedCashes.FirstOrDefault(c => c.CostCenter == CostCenters.Transportation);
            if (transportationCash is not null)
            {
                transportationCash.MoneyTransactionType = sale.CashState == CashStates.NonCash ? MoneyTransactionTypes.NonCashDelivery : MoneyTransactionTypes.CashDelivery;
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

            var relatedCashes = CashRepository.Find(c => (c.MoneyTransactionType == MoneyTransactionTypes.CashSale ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashSale ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.CashIndirectSale ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashIndirectSale ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashIndirectBuy ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.CashDelivery ||
                                                            c.MoneyTransactionType == MoneyTransactionTypes.NonCashDelivery) && salesIds.Contains(c.RelatedEntityId)).Take(3);
            CashRepository.RemoveRange(relatedCashes);
        }
    }
}
