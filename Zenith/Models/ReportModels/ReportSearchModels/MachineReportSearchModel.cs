using ReactiveUI.Fody.Helpers;
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
        public DateTime StartDate { get; set; }

        [Reactive]
        public DateTime EndDate { get; set; }
    }
}
