using ReactiveUI.Fody.Helpers;
using System;
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
    }
}
