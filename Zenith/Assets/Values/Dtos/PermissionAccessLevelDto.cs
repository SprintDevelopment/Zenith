using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Zenith.Assets.Values.Enums;

namespace Zenith.Assets.Values.Dtos
{
    public class PermissionAccessLevelDto : DependencyObject, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public PermissionTypes PermissionType
        {
            get { return (PermissionTypes)base.GetValue(PermissionTypeProperty); }
            set { base.SetValue(PermissionTypeProperty, value); NotifyPropertyChanged(nameof(PermissionType)); }
        }
        public static readonly DependencyProperty PermissionTypeProperty = DependencyProperty.Register("PermissionType", typeof(PermissionTypes), typeof(PermissionAccessLevelDto), new PropertyMetadata(PermissionTypes.DontCare));

        public AccessLevels AccessLevel { get; set; }
    }
}
