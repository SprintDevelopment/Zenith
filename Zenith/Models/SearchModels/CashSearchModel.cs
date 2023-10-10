using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class CashSearchModel : SearchBaseDto
    {
        //[Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(TransferDirections))]
        //[Reactive]
        //public TransferDirections TransferDirection { get; set; } = TransferDirections.DontCare;

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CompanyRepository))]
        [Reactive]
        public short CompanyId { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(MoneyTransactionTypes))]
        [Reactive]
        public MoneyTransactionTypes MoneyTransactionType { get; set; } = MoneyTransactionTypes.DontCare;

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CostCenters))]
        [Reactive]
        public CostCenters CostCenter { get; set; } = CostCenters.DontCare;
    }
}
