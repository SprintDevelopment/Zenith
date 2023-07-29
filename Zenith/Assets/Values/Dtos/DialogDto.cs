using Zenith.Assets.Values.Enums;
using System.Collections.Generic;

namespace Zenith.Assets.Values.Dtos
{
    public class DialogDto : BaseDto
    {
        public DialogTypes DialogType { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<DialogChoiceDto> Choices { get; set; }
    }
}
