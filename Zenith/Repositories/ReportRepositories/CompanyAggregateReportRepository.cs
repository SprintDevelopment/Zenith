using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;

namespace Zenith.Repositories.ReportRepositories
{
    public class CompanyAggregateReportRepository : ReportRepository<CompanyAggregateReport>
    {
        public override IEnumerable<CompanyAggregateReport> Find(BaseDto searchModel)
        {
            var reportSearchModel = (CompanyAggregateReportSearchModel)searchModel;
            var companyName = _context.Set<Company>().Find(reportSearchModel.CompanyId).Name;
            
            var sites = _context.Set<Site>().Where(s => s.CompanyId == reportSearchModel.CompanyId).ToList();
            var sitesIds = reportSearchModel.SiteId == 0 ?
                sites.Select(s => s.SiteId) :
                new List<int> { reportSearchModel.SiteId };

            return _context.Set<Delivery>()
                .Include(d => d.SaleItem)
                .Where(d => d.DateTime.Year == reportSearchModel.Year
                    && d.DateTime.Month == (int)reportSearchModel.Month
                    && sitesIds.Contains(d.SiteId))
                .GroupBy(d => d.SiteId)
                .Select(g => new CompanyAggregateReport
                {
                    CompanyName = companyName,
                    Year = reportSearchModel.Year,
                    Month = reportSearchModel.Month,
                    SiteId = g.Key,
                    InvoiceNo = "12345",
                    TotalAmount = g.Sum(d => d.DeliveryFee + (d.SaleItem.UnitPrice * d.SaleItem.Count)),
                    CountUnitTitle = "Trip"
                })
                .Union(_context.Set<MachineIncome>().Where(mi => mi.DateTime.Year == reportSearchModel.Year
                    && mi.DateTime.Month == (int)reportSearchModel.Month
                    && mi.SiteId != null
                    && sitesIds.Contains(mi.SiteId.Value)).GroupBy(d => d.SiteId)
                    .Select(g => new CompanyAggregateReport
                    {
                        CompanyName = companyName,
                        Year = reportSearchModel.Year,
                        Month = reportSearchModel.Month,
                        SiteId = g.Key.Value,
                        InvoiceNo = "12345",
                        TotalAmount = g.Sum(d => d.Value),
                        CountUnitTitle = "Hour"
                    }))
                .AsEnumerable()
                .Select(r => { r.SiteName = sites.Single(s => s.SiteId == r.SiteId).Name; return r; });
        }
    }
}
