using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class SalaryPaymentSearchModel : SearchBaseDto
    {
        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(PersonRepository))]
        [Reactive]
        public int PersonId { get; set; }


        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CostCenters))]
        [Reactive]
        public CostCenters CostCenter { get; set; } = CostCenters.DontCare;

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(DateRanges))]
        [Reactive]
        public DateRanges DateRange { get; set; } = DateRanges.Today;
    }
}
