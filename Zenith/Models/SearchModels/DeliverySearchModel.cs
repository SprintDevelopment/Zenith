using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class DeliverySearchModel : SearchBaseDto
    {
        [Search]
        [Reactive]
        public string DeliveryNumber { get; set; }

        [Search]
        [Reactive]
        public string LpoNumber { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(MaterialRepository))]
        [Reactive]
        public int MaterialId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CompanyRepository))]
        [Reactive]
        public short CompanyId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(SiteRepository))]
        [Reactive]
        public int SiteId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(MachineRepository))]
        [Reactive]
        public int MachineId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(PersonRepository))]
        [Reactive]
        public int DriverId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(DateRanges))]
        [Reactive]
        public DateRanges DateRange { get; set; } = DateRanges.ThisWeek;
    }
}
