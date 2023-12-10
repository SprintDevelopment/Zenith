using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models.ReportModels;
using Zenith.Models.ReportModels.ReportSearchModels;
using Zenith.Repositories.ReportRepositories;

namespace Zenith.ViewModels.ReportViewModels
{
    public class CompanyAggregateReportViewModel : BaseReportViewModel<CompanyAggregateReport>
    {
        public CompanyAggregateReportViewModel(ReportRepository<CompanyAggregateReport> repository, BaseDto searchModel, PermissionTypes permissionType)
            : base(repository, searchModel, permissionType)
        {

            PrintCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                WordUtil.PrintCompanyAggregateReport(((CompanyAggregateReportSearchModel)searchModel).CompanyId, ActiveList.ToObservableCollection());
            });
        }
    }
}
