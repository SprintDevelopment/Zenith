using ReactiveUI;
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
        private string subject;

        [Search(Title = "موضوع یادداشت")]
        public string Subject
        {
            get { return subject; }
            set { this.RaiseAndSetIfChanged(ref subject, value); }
        }
    }
}
