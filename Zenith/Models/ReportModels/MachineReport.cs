using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.ReportModels
{
    public class MachineReport : ReportModel
    {
        [Reactive]
        public string MachineName { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public TransferDirections TransferDirection { get; set; }

        [Reactive]
        public float Value { get; set; }

        [Reactive]
        public float Remained { get; set; }

        [Reactive]
        public DateTime DateTime { get; set; }

        [Reactive]
        public string MoreInfo { get; set; }

        [Reactive]
        public string Comment { get; set; }

    }
}
