using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Zenith.Assets.Extensions;

namespace Zenith.Assets.UI.BaseClasses
{
    public class JTextBox : FloatHandledTextBox, INotifyPropertyChanged, IDisposable
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

        public bool LeftToRightText
        {
            get { return (bool)base.GetValue(LeftToRightTextProperty); }
            set { base.SetValue(LeftToRightTextProperty, value); NotifyPropertyChanged(nameof(LeftToRightText)); }
        }
        public static readonly DependencyProperty LeftToRightTextProperty = DependencyProperty.Register("LeftToRightText", typeof(bool), typeof(JTextBox), new PropertyMetadata(false));

        public JTextBox()
        {
        }
    }
}
