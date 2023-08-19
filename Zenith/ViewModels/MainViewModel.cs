using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Models;

namespace Zenith.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            ShowDialog = ReactiveCommand.Create<DialogDto>(dialogDto =>
            {
                DialogResult = DialogResults.None;
                DialogDto = dialogDto;
            });

            Navigate = ReactiveCommand.Create<Type>(type =>
            {
                ListPage = (Page)Activator.CreateInstance(type);
            });

            ShowCreateUpdatePage = ReactiveCommand.Create<Page>(page =>
            {
                CreateUpdatePage = page;
            });

            CreateUpdatePageReturned = ReactiveCommand.Create<Unit>(_ =>
            {
                CreateUpdatePage = null;
            });

            InitiateSearch = ReactiveCommand.Create<SearchBaseDto, SearchBaseDto>(model =>
            {
                return model;
            }, this.WhenAnyValue(vm => vm.IsLocked).Where(il => !il));

            OpenLogFile = ReactiveCommand.Create<string>(_ =>
            {
                Process.Start("notepad.exe", $"C:\\{_}");
            });

            Backup = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                var backupResult = DatabaseUtil.Backup(@"D:\Backups\");
                _alerts.Add(new AlertViewModel
                {
                    Guid = new Guid(),
                    Title = backupResult.ResultTitle,
                    Description = backupResult.ResultDescription,
                    DialogType = backupResult.OperationResultType == OperationResultTypes.Succeeded ? DialogTypes.Success : DialogTypes.Danger,
                    ActionContent = backupResult.OperationResultType == OperationResultTypes.Succeeded ?
                        "مشاهده فایل پشتیبان" : "مشاهده فایل لاگ برنامه",
                    ActionCommand = ReactiveCommand.Create<Unit>(_ =>
                    {
                        if (backupResult.OperationResultType == OperationResultTypes.Succeeded)
                            Process.Start("explorer.exe", $"/select, \"{backupResult.UsefulParameter}\"");
                        else
                            Process.Start("notepad.exe", @"C:\file.txt");
                    })
                });

            });

            Restore = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
            {
                var restoreResult = DatabaseUtil.Restore(@"D:\Backups\");
                if (restoreResult is not null)
                {
                    _alerts.Add(new AlertViewModel
                    {
                        Guid = new Guid(),
                        Title = restoreResult.ResultTitle,
                        Description = restoreResult.ResultDescription,
                        DialogType = restoreResult.OperationResultType == OperationResultTypes.Succeeded ? DialogTypes.Success : DialogTypes.Danger,
                        ActionContent = restoreResult.OperationResultType == OperationResultTypes.Succeeded ? string.Empty : "مشاهده فایل لاگ برنامه",
                        ActionCommand = restoreResult.OperationResultType == OperationResultTypes.Succeeded ?
                            null : ReactiveCommand.Create<Unit>(_ => { Process.Start("notepad.exe", @"C:\file.txt"); })
                    });
                }
            });

            _alerts.Connect()
                .Bind(out Alerts)
                .Subscribe();

            _alerts.Connect()
                .MergeMany(t => t.CloseCommand)
                .Select(guid => _alerts.Items.FirstOrDefault(t => t.Guid == guid))
                .Do(tabToRemove => _alerts.Remove(tabToRemove))
                .Subscribe();
        }

        [Reactive]
        public User LoggedInUser { get; set; }

        [Reactive]
        public bool IsMenuVisible { get; set; }

        [Reactive]
        public bool IsSearchVisible { get; set; }

        [Reactive]
        public bool IsLocked { get; set; }

        [Reactive]
        public Page ListPage { get; set; }

        [Reactive]
        public Page CreateUpdatePage { get; set; }

        [Reactive]
        public DialogDto DialogDto { get; set; }

        [Reactive]
        public DialogResults DialogResult { get; set; }

        public SourceList<AlertViewModel> _alerts { get; private set; } = new SourceList<AlertViewModel>();
        public ReadOnlyObservableCollection<AlertViewModel> Alerts;

        public ReactiveCommand<Type, Unit> Navigate { get; set; }
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePage { get; set; }
        public ReactiveCommand<DialogDto, Unit> ShowDialog { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturned { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearch { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFile { get; set; }
        public ReactiveCommand<Unit, Unit> Backup { get; set; }
        public ReactiveCommand<Unit, Unit> Restore { get; set; }
    }
}
