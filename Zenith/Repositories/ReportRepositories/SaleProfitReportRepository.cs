using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Models;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;

namespace Zenith.Repositories.ReportRepositories
{
    public class SaleProfitReportRepository : ReportRepository<SaleProfitReportModel>
    {
        public override IEnumerable<SaleProfitReportModel> Find(BaseDto searchModel)
        {
            var reportSearchModel = (SaleProfitReportSearchModel)searchModel;

            var saleItems = _context.Set<Sale>()
                //.Where(s => s.DateTime.Year == reportSearchModel.Year
                //    && s.DateTime.Month == (int)reportSearchModel.Month)
                .Include(s => s.Items)
                .SelectMany(s => s.Items.Select(si => new {si.Material, si.Count, si.UnitPrice, isOld = s.DateTime < new DateTime(reportSearchModel.Year, (int)reportSearchModel.Month, 1)}))
                .GroupBy(si => new { si.Material, si.isOld  })
                .Select(g => new { materialId = g.Key, soldCount = g.Sum(x => x.Count), soldPrice = g.Sum(x => x.UnitPrice) });
        }
    }
}
