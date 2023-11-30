using Microsoft.WindowsAPICodePack.Net;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Models;

namespace Zenith.Assets.Values.Dtos
{
    public class SalesPrePrintDto : BaseDto
    {
        [Reactive]
        public ObservableCollection<Material> Materials { get; set; }

        [Reactive]
        public ObservableCollection<Site> Sites { get; set; }

        [Reactive]
        public bool FilteredBySite { get; set; }

        [Reactive]
        public bool SeparatedBySite { get; set; }

        [Reactive]
        public bool FilteredByMaterial { get; set; }

        [Reactive]
        public bool SeparatedByMaterial { get; set; }

        [Reactive]
        public bool FilteredByLpo { get; set; }

        [Reactive]
        public string LpoNumber { get; set; }

        [Reactive]
        public bool FactorNumberRequired { get; set; }

        [Reactive]
        public int FactorNumber { get; set; }

        [Reactive]
        public string SearchedSiteName { get; set; }

        [Reactive]
        public string SearchedMaterialName { get; set; }

        public SalesPrePrintDto()
        {
        }
    }
}
