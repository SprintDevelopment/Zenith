using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        public LoginViewModel()
        {
            var mainViewModel = App.MainViewModel;

            var userRepository = new UserRepository();

            LoginCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                mainViewModel.LoggedInUser = userRepository.Find(u => u.Username == Username && u.HashedPassword == CryptoUtil.GenerateSaltedHashBytes(Password)).FirstOrDefault();
                if (mainViewModel.LoggedInUser != null)
                    mainViewModel.CreateUpdatePageReturnedCommand.Execute().Subscribe();
            }, this.WhenAnyValue(vm => vm.Username, vm => vm.Password)
                .Select(cr => !(cr.Item1.IsNullOrWhiteSpace() || cr.Item2.IsNullOrWhiteSpace())));

            CloseCommand = ReactiveCommand.Create<Unit>(_ => App.Current.Shutdown());
        }

        [Reactive]
        public string Username { get; set; }

        [Reactive]
        public string Password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    }
}
