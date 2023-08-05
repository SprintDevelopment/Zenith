using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;

namespace Zenith.Models.SearchModels
{
    public class NoteSearchModel : SearchBaseDto
    {
        [Search(Title = "موضوع یادداشت")]
        [Reactive]
        public string Subject { get; set; }
    }
}
