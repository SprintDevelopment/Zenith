using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Assets.Values.Dtos
{
    public class BaseDto : ReactiveValidationObject, IDisposable
    {
        #region Dispose
        public void Dispose() { }
        #endregion

        [Reactive]
        [NotMapped]
        public int DisplayOrder { get; set; }

        [Reactive]
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
