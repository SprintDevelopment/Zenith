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
    public class MachineReportRepository : ReportRepository<MachineReport>
    {
        public override IEnumerable<MachineReport> Find(BaseDto searchModel)
        {
            var reportSearchModel = (MachineReportSearchModel)searchModel;
            var machineName = _context.Set<Machine>().Find(reportSearchModel.MachineId).Name;

            IEnumerable<MachineReport> query = Enumerable.Empty<MachineReport>();

            if (reportSearchModel.MachineReportType != Assets.Values.Enums.MachineReportTypes.OnlyOutgoes)
                query = query.Concat(_context.Set<Delivery>()
                     .Where(d => d.MachineId == reportSearchModel.MachineId)
                     .Include(d => d.Site).ThenInclude(s => s.Company)
                     .Include(d => d.SaleItem).ThenInclude(si => si.Material)
                     .Select(d => new MachineReport
                     {
                         MachineName = machineName,
                         Title = "Delivery",
                         TransferDirectionSign = +1,
                         Value = d.DeliveryFee,
                         DateTime = d.DateTime,
                         MoreInfo = $"Company: {d.Site.Company.Name}, Site : {d.Site.Name}"
                     }).AsEnumerable());

            if (reportSearchModel.MachineReportType != Assets.Values.Enums.MachineReportTypes.OnlyDeliveries)
                query = query.Concat(_context.Set<MachineOutgo>()
                    .Where(mo => mo.MachineId == reportSearchModel.MachineId)
                    .Include(mo => mo.Company)
                    .Include(mo => mo.OutgoCategory)
                    .Select(mo => new MachineReport
                    {
                        MachineName = machineName,
                        Title = "Machine outgo",
                        TransferDirectionSign = -1,
                        Value = mo.Value,
                        DateTime = mo.DateTime,
                        MoreInfo = $"Company: {mo.Company.Name}, Category : {mo.OutgoCategory.Title}"
                    }).AsEnumerable());

            var sum = 0f;
            return query
                .OrderBy(r => r.DateTime)
                .Select(r =>
                {
                    sum += r.Value * r.TransferDirectionSign;
                    r.Remained = sum;
                    return r;
                });
        }
    }
}
