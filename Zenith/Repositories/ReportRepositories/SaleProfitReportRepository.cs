using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;

namespace Zenith.Repositories.ReportRepositories
{
    public class SaleProfitReportRepository : ReportRepository<SaleProfitReport>
    {
        public override IEnumerable<SaleProfitReport> Find(BaseDto searchModel)
        {
            var reportSearchModel = (SaleProfitReportSearchModel)searchModel;

            return _context.Set<Sale>()
                .Where(s => s.DateTime < new DateTime(reportSearchModel.Year, (int)reportSearchModel.Month + 1, 1))
                .Include(s => s.Items).ThenInclude(si => si.Material)
                .SelectMany(s => s.Items.Where(si => !si.Material.IsMixed).Select(si => new { si.MaterialId, si.Material.Name, si.Count, si.UnitPrice, isForPrevSales = s.DateTime < new DateTime(reportSearchModel.Year, (int)reportSearchModel.Month, 1) }))
                .GroupBy(si => new { si.MaterialId })
                .Select(g => new
                {
                    g.Key.MaterialId,
                    materialName = g.First().Name,
                    prevSoldCount = g.Where(i => i.isForPrevSales).Sum(x => x.Count),
                    thisMonthSoldCount = g.Where(i => !i.isForPrevSales).Sum(x => x.Count),
                    thisMonthSoldPrice = g.Where(i => !i.isForPrevSales).Sum(x => x.UnitPrice * x.Count)
                })
                .AsEnumerable()
                .SelectMany(x =>
                {
                    var oldCount = x.prevSoldCount;
                    var newCount = x.thisMonthSoldCount;

                    return _context.Set<BuyItem>().Include(bi => bi.Buy).Where(bi => bi.MaterialId == x.MaterialId)
                    .Select(bi => new { bi.MaterialId, bi.UnitPrice, bi.Count, bi.Buy.DateTime })
                    .OrderBy(bi => bi.DateTime)
                    .AsEnumerable()
                    .SkipWhile(bi => oldCount >= bi.Count ? (oldCount -= bi.Count) >= 0 : false)
                    .TakeWhile(bi => newCount > 0)
                    .DefaultIfEmpty(new { x.MaterialId, UnitPrice = 0f, Count = 0f, DateTime = DateTime.Now })
                    .Select(bi =>
                    {
                        var res = new
                        {
                            count = Math.Min(newCount, bi.Count - oldCount),
                            bi.UnitPrice,
                            bi.MaterialId
                        };

                        oldCount = 0;
                        newCount -= res.count;

                        return res;
                    }).GroupBy(i => i.MaterialId)
                    .Select(g => new SaleProfitReport
                    {
                        Year = reportSearchModel.Year,
                        Month = reportSearchModel.Month,
                        MaterialName = x.materialName,
                        SoldPrice = x.thisMonthSoldPrice,
                        SoldCount = x.thisMonthSoldCount,
                        BoughtPrice = g.Sum(i => i.UnitPrice * i.count),
                        IsValid = newCount == 0
                    });
                });
        }
    }
}
