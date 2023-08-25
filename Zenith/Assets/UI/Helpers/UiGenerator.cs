﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Zenith.Assets.UI.BaseClasses;

namespace Zenith.Assets.UI.Helpers
{
    public class UiGenerator
    {
        public static JTextBox GetTextBox(string title)
        {
            return new JTextBox { Title = title, Height = 42 };
        }

        public static ComboBox GetComboBox(string title)
        {
            return new ComboBox { Tag = title, Height = 42 };
        }
    }
}
