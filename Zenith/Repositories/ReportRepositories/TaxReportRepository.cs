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
    public class TaxReportRepository : ReportRepository<TaxReport>
    {
        public override IEnumerable<TaxReport> Find(BaseDto searchModel)
        {
            var reportSearchModel = (TaxReportSearchModel)searchModel;

            return Enumerable.Empty<TaxReport>().Append(new TaxReport
            {
                TaxedBuyAmount = _context.Set<Buy>()
                    .Where(b => b.DateTime >= reportSearchModel.StartDate && b.DateTime <= reportSearchModel.EndDate)
                    .SelectMany(b => b.Items)
                    .Sum(bi => bi.UnitPrice * bi.Count),

                TaxedOutgoAmount = _context.Set<Outgo>()
                    .Where(o => o.DateTime >= reportSearchModel.StartDate && o.DateTime <= reportSearchModel.EndDate)
                    .Sum(o => o.Value),
                
                TaxedSaleAmount = _context.Set<Sale>()
                    .Include(s => s.Company)
                    .Where(s => s.Company.IsTaxPayer && s.DateTime >= reportSearchModel.StartDate && s.DateTime <= reportSearchModel.EndDate)
                    .SelectMany(s => s.Items)
                    .Sum(si => si.UnitPrice * si.Count),
            });
        }
    }
}
