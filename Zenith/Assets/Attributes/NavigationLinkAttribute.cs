using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;

namespace Zenith.Assets.Attributes
{
    public class NavigationLinkAttribute : Attribute, IDisposable, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged, Dispose
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose() { }
        #endregion

        private string _title;
        private string _navigatePageSource;
        private string _shortcut;
        private string _iconSource;

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged(); }
        }

        public string Shortcut
        {
            get { return _shortcut; }
            set { _shortcut = value; NotifyPropertyChanged(); }
        }

        public string NavigationPageSource
        {
            get { return _navigatePageSource; }
            set { _navigatePageSource = value; NotifyPropertyChanged(); }
        }

        public string Icon
        {
            get { return _iconSource; }
            set { _iconSource = value; NotifyPropertyChanged(); }
        }

        public NavigationLinkAttribute()
        {
            Icon = "";
        }
    }
}
