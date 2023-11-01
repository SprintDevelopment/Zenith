using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class MachineIncomeSearchModel : SearchBaseDto
    {
        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(MachineRepository))]
        [Reactive]
        public int MachineId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(SiteRepository))]
        [Reactive]
        public int SiteId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(IncomeCategoryRepository))]
        [Reactive]
        public short IncomeCategoryId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CompanyRepository))]
        [Reactive]
        public short CompanyId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(DateRanges))]
        [Reactive]
        public DateRanges DateRange { get; set; } = DateRanges.Today;
    }
}
