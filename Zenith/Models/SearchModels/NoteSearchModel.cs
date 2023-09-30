using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.Models.SearchModels
{
    public class NoteSearchModel : SearchBaseDto
    {
        [Search]
        [Reactive]
        public string Subject { get; set; }

        [Search(ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(NotifyTypes))]
        [Reactive]
        public NotifyTypes NotifyType { get; set; } = NotifyTypes.DontCare;
    }
}
