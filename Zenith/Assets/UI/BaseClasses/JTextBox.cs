using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTextBox : TextBox, INotifyPropertyChanged, IDisposable
    {
        #region NotifyPropertyChanged, Dispose
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose() { }
        #endregion

        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); NotifyPropertyChanged(nameof(Title)); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(JTextBox), new PropertyMetadata(""));

        public JTextBox()
        {
            this.GotFocus += (s, e) => { SelectAll(); };
        }
    }
}
