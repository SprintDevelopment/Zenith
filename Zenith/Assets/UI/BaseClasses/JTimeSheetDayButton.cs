using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTimeSheetDayButton : Button
    {
        public bool IsAbsent
        {
            get { return (bool)base.GetValue(IsAbsentProperty); }
            set { base.SetValue(IsAbsentProperty, value); }
        }
        public static readonly DependencyProperty IsAbsentProperty = DependencyProperty.Register("IsAbsent", typeof(bool), typeof(JTimeSheetDayButton), new PropertyMetadata(false));

        public string Overtime
        {
            get { return (string)base.GetValue(OvertimeProperty); }
            set { base.SetValue(OvertimeProperty, value); }
        }
        public static readonly DependencyProperty OvertimeProperty = DependencyProperty.Register("Overtime", typeof(string), typeof(JTimeSheetDayButton), new PropertyMetadata(""));
    }
}
