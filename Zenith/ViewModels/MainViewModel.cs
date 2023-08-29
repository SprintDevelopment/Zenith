using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Data;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;

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
                var tabForPage = TabControlViewModel._tabs.Items.FirstOrDefault(tab => tab.RelatedMainPage.GetType() == type);
                if (tabForPage is not null)
                    tabForPage.IsSelected = true;
                else
                    TabControlViewModel._tabs.Add(new TabViewModel { RelatedMainPage = (Page)Activator.CreateInstance(type), IsSelected = true });
            });


            TabControlViewModel.WhenAnyValue(vm => vm.SelectedTabViewModel)
                .Where(stvm => stvm?.RelatedMainPage is not null)
                .Do(stvm => ListPage = stvm.RelatedMainPage)
                .Subscribe();


            ShowDeliveriesCommand = ReactiveCommand.Create<Sale>(sale =>
            {
                ListPage = new DeliveryListPage(sale);
            });

            ShowCreateUpdatePage = ReactiveCommand.Create<Page>(page =>
            {
                CreateUpdatePage = page;
            });

            CreateUpdatePageReturned = ReactiveCommand.Create<Unit>(_ =>
            {
                CreateUpdatePage = null;
            });

            ShowSecondCreateUpdatePage = ReactiveCommand.Create<Page>(page =>
            {
                SecondCreateUpdatePage = page;
            });

            SecondCreateUpdatePageReturned = ReactiveCommand.Create<Unit>(_ =>
            {
                SecondCreateUpdatePage = null;
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

            CreateDatabase = ReactiveCommand.CreateRunInBackground<Unit>(_ => new DbContextFactory().CreateDbContext(null).Database.Migrate());

            _alerts.Connect()
                .Bind(out Alerts)
                .Subscribe();

            _alerts.Connect()
                .MergeMany(t => t.CloseCommand)
                .Select(guid => _alerts.Items.FirstOrDefault(t => t.Guid == guid))
                .Do(tabToRemove => _alerts.Remove(tabToRemove))
                .Subscribe();

            var noteRepository = new NoteRepository();
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMinutes(1))
                .SelectMany(_ => noteRepository.Find(note => note.NotifyType == NotifyTypes.FooterNotify && note.NotifyDateTime.CompareTo(DateTime.Now) <= 0))
                .Select(note => 
                {
                    note.NotifyType = NotifyTypes.NoNeedToNotify;
                    noteRepository.Update(note, note.NoteId);
                    //noteRepository.SaveChanges();

                    return new AlertViewModel
                    {
                        Guid = new Guid(),
                        Title = note.Subject,
                        Description = !note.Comment.IsNullOrWhiteSpace() ? note.Comment : $"موعد یادداشتی با عنوان {note.Subject} رسیده است.",
                        DialogType = DialogTypes.Info,
                        ActionContent = "متوجه شدم",
                        ActionCommand = ReactiveCommand.Create<Unit>(_ => { })
                    };
                }).Do(alert => _alerts.Add(alert))
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
        public Page SecondCreateUpdatePage { get; set; }

        [Reactive]
        public DialogDto DialogDto { get; set; }

        [Reactive]
        public DialogResults DialogResult { get; set; }

        public SourceList<AlertViewModel> _alerts { get; private set; } = new SourceList<AlertViewModel>();
        public ReadOnlyObservableCollection<AlertViewModel> Alerts;
        public TabControlViewModel TabControlViewModel { get; set; } = new TabControlViewModel();
        public ReactiveCommand<Type, Unit> Navigate { get; set; }
        public ReactiveCommand<Sale, Unit> ShowDeliveriesCommand { get; set; }
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePage { get; set; }
        public ReactiveCommand<Page, Unit> ShowSecondCreateUpdatePage { get; set; }
        public ReactiveCommand<DialogDto, Unit> ShowDialog { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturned { get; set; }
        public ReactiveCommand<Unit, Unit> SecondCreateUpdatePageReturned { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearch { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFile { get; set; }
        public ReactiveCommand<Unit, Unit> Backup { get; set; }
        public ReactiveCommand<Unit, Unit> Restore { get; set; }
        public ReactiveCommand<Unit, Unit> CreateDatabase { get; set; }
    }
}
