using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.SearchModels
{
    public class PersonSearchModel : SearchBaseDto
    {
        [Search]
        [Reactive]
        public string Name { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CostCenters))]
        [Reactive]
        public Jobs Job { get; set; } = Jobs.DontCare;

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CostCenters))]
        [Reactive]
        public CostCenters CostCenter { get; set; } = CostCenters.DontCare;
    }
}
