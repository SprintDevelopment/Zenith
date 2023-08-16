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

            ViewModel = new BaseCreateOrUpdateViewModel<User>(new UserRepository());

            this.WhenActivated(d =>
            {
                Observable
                    .Merge(
                        Observable.FromEventPattern(passPasswordBox, nameof(PasswordBox.PasswordChanged)),
                        Observable.FromEventPattern(repeatPassPasswordBox, nameof(PasswordBox.PasswordChanged)))
                    .Where(_ => passPasswordBox.Password == repeatPassPasswordBox.Password)
                    .Do(_ => ViewModel.PageModel.Password = passPasswordBox.Password)
                    .Subscribe().DisposeWith(d);

                Observable.FromEventPattern(addImageButton, nameof(Button.Click))
                    .Do(_ =>
                    {
                        var openFileDialog = new OpenFileDialog()
                        {
                            Filter = "Image Files(*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png"
                        };

                        var result = openFileDialog.ShowDialog();
                        if (result.HasValue && result.Value)
                        {
                            ViewModel.PageModel.AvatarImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                        }
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
