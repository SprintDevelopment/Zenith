using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenith.Assets.Extensions;
using Zenith.Models;
using Zenith.Repositories;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;
using System.Reactive;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class UserCreateOrUpdateViewModel : BaseCreateOrUpdateViewModel<User>
    {
        public UserCreateOrUpdateViewModel(Repository<User> repository, bool containsDeleted = false)
            : base(repository, containsDeleted)
        {
            ToggleOneCommand = ReactiveCommand.Create<PermissionAccessLevelDto>(permissionAccessLevelDto =>
            {
                var permission = PageModel.Permissions.First(p => p.PermissionType == permissionAccessLevelDto.PermissionType);

                _ = (permissionAccessLevelDto.AccessLevel) switch
                {
                    AccessLevels.CanRead => permission.HasReadAccess = !permission.HasReadAccess,
                    AccessLevels.CanCreate => permission.HasCreateAccess = !permission.HasCreateAccess,
                    AccessLevels.CanUpdate => permission.HasUpdateAccess = !permission.HasUpdateAccess,
                    AccessLevels.CanDelete => permission.HasDeleteAccess = !permission.HasDeleteAccess
                };
            });

            //ToggleAllCommand = ReactiveCommand.Create<AccessLevels>(accessLevel =>
            //{
            //    var allSelected = PageModel.Permissions.Any(p => p.)

            //    PageModel.Permissions.Select(p => (accessLevel) switch
            //    {
            //        AccessLevels.CanRead => p.HasReadAccess = !p.HasReadAccess;
            //        AccessLevels.CanCreate => p.HasCreateAccess = !p.HasCreateAccess;
            //        AccessLevels.CanUpdate => p.HasUpdateAccess = !p.HasUpdateAccess;
            //        AccessLevels.CanDelete => p.HasDeleteAccess = !p.HasDeleteAccess;
            //    }).ToList();
            //});
        }

        public SourceList<Material> MaterialsSourceList { get; set; } = new SourceList<Material>();
        public ReadOnlyObservableCollection<Material> MaterialsActiveList;

        [Reactive]
        public string SearchedMaterialName { get; set; }

        public ReactiveCommand<PermissionAccessLevelDto, Unit> ToggleOneCommand { get; set; }
        public ReactiveCommand<AccessLevels, Unit> ToggleAllCommand { get; set; }
    }
}
