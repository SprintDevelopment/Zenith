using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Values.Dtos
{
    public class AppLicenseDto : BaseDto
    {
        [Reactive]
        public string SerialNumber { get; set; }
        
        [Reactive]
        public DateTime StartDate { get; set; }
       
        [Reactive]
        public DateTime EndDate { get; set; }
        
        [Reactive]
        public AppLicenseStates State { get; set; }

        [Reactive] 
        public bool IsLicenseValid { get; set; }

        public AppLicenseDto()
        {
            this.WhenAnyValue(dto => dto.State)
                .Select(s => s == AppLicenseStates.Valid)
                .BindTo(this, dto => dto.IsLicenseValid);
        }
    }
}
