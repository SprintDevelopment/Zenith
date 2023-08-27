using Microsoft.Win32;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.CreateOrUpdateViewModels;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : BaseCreateOrUpdatePage<User>
    {
        public UserPage()
        {
            InitializeComponent();

            ViewModel = new UserCreateOrUpdateViewModel(new UserRepository());

            this.WhenActivated(d =>
            {
                var newPermissions = typeof(PermissionTypes).ToCollection().Where(p => !ViewModel.PageModel.Permissions.Any(up => up.PermissionType == (PermissionTypes)p.Value)).ToList();
                newPermissions.ForEach(p => ViewModel.PageModel.Permissions.Add(new UserPermission { PermissionType = (PermissionTypes)p.Value }));

                Observable
                    .Merge(
                        Observable.FromEventPattern(passPasswordBox, nameof(PasswordBox.PasswordChanged)),
                        Observable.FromEventPattern(repeatPassPasswordBox, nameof(PasswordBox.PasswordChanged)))
                    .Where(_ => passPasswordBox.Password == repeatPassPasswordBox.Password)
                    .Do(_ => ViewModel.PageModel.Password = passPasswordBox.Password)
                    .Subscribe().DisposeWith(d);
            });
        }
    }
}
