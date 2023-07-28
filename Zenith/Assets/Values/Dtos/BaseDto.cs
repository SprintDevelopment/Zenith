using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using System;
using System.Collections.Generic;
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

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { this.RaiseAndSetIfChanged(ref isSelected, value); }
        }
    }
}
