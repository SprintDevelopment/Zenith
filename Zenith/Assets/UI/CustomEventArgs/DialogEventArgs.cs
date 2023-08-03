using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.UI.CustomEventArgs
{
    public class DialogEventArgs : EventArgs
    {
        public DialogResults DialogResult { get; set; }
    }
}
