using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Zenith.Assets.Extensions
{
    public static class KeyExtensions
    {
        public static char ToChar(this Key key)
        {
            return key switch
            {
                >= Key.D0 and <= Key.D9 => (char)(48 + key - Key.D0),
                >= Key.NumPad0 and <= Key.NumPad9 => (char)(48 + key - Key.NumPad0),
                >= Key.A and <= Key.Z => (char)(65 + key - Key.A),
                Key.Decimal or Key.OemPeriod => '.',
                Key.Space => ' ',
                _ => char.MinValue
            };
        }
    }
}
