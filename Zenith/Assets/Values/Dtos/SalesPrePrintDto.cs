using Microsoft.WindowsAPICodePack.Net;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Assets.Values.Dtos
{
    public class SalesPrePrintDto : BaseDto
    {
        public List<Material> Materials { get; set; }
        public List<Site> Sites { get; set; }

        [Reactive]
        public bool FilteredBySite { get; set; }

        [Reactive]
        public bool SeparatedBySite { get; set; }

        [Reactive]
        public bool FilteredByMaterial { get; set; }

        [Reactive]
        public bool SeparatedByMaterial { get; set; }

        [Reactive]
        public bool FilterByLpo { get; set; }
    }
}
