using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class MachineOutgoSearchModel : SearchBaseDto
    {
        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(MachineRepository))]
        [Reactive]
        public int MachineId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(OutgoCategoryRepository))]
        [Reactive]
        public short OutgoCategoryId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CompanyRepository))]
        [Reactive]
        public short CompanyId { get; set; }
    }
}
