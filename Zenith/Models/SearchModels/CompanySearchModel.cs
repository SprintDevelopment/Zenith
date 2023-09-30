using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Models.SearchModels
{
    public class CompanySearchModel : SearchBaseDto
    {
        [Search]
        [Reactive]
        public string Name { get; set; }
    }
}
