using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Models.SearchModels
{
    public class CompanySearchModel : SearchBaseDto
    {
        [Search(Title = "نام شرکت")]
        [Reactive]
        public string Name { get; set; }
    }
}
