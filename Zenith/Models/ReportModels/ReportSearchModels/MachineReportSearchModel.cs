using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels.ReportSearchModels
{
    public class MachineReportSearchModel : BaseDto
    {
        [Reactive]
        public int MachineId { get; set; }
        
        [Reactive]
        public MachineReportTypes MachineReportType { get; set; }

        [Reactive]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Reactive]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public MachineReportSearchModel()
        {
            this.ValidationRule(vm => vm.MachineId, mi => mi > 0, "Select machine");
        }
    }
}
