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
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.ViewModels
{
    public class LicenseViewModel : ReactiveObject
    {
        public LicenseViewModel()
        {
            var mainViewModel = App.MainViewModel;

            GetAppLicenseCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ => MapperUtil.Mapper.Map(LicenseUtil.GetLicense(), License));

            CloseCommand = ReactiveCommand.Create<Unit>(_ => App.Current.Shutdown());
        }

        [Reactive]
        public AppLicenseDto License { get; set; } = new AppLicenseDto();

        [Reactive]
        public string Password { get; set; }

        public ReactiveCommand<Unit, Unit> GetAppLicenseCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    }
}
