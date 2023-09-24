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
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Data;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.Views.ListViews;
using Zenith.Views.ReportViews;

namespace Zenith.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            ShowDialogCommand = ReactiveCommand.Create<DialogDto>(dialogDto =>
            {
                DialogResult = DialogResults.None;
                DialogDto = dialogDto;
            });

            NavigateCommand = ReactiveCommand.Create<Type>(type =>
            {
                var tabForPage = TabControlViewModel._tabs.Items.FirstOrDefault(tab => tab.RelatedMainPage.GetType() == type);
                if (tabForPage is not null)
                    tabForPage.IsSelected = true;
                else
                    TabControlViewModel._tabs.Add(new TabViewModel { RelatedMainPage = (Page)Activator.CreateInstance(type), IsSelected = true });
            });

            NavigateToBuysCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(BuyListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Buys && p.HasReadAccess)));
            NavigateToSalesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(SaleListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Sales && p.HasReadAccess)));
            NavigateToAccountsCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(AccountListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Accounts && p.HasReadAccess)));
            NavigateToCashesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(CashListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Cashes && p.HasReadAccess)));
            NavigateToChequesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(ChequeListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Cheques && p.HasReadAccess)));
            NavigateToCompaniesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(CompanyListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Companies && p.HasReadAccess)));
            NavigateToSitesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(SiteListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Sites && p.HasReadAccess)));
            NavigateToMaterialsCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(MaterialListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Materials && p.HasReadAccess)));
            NavigateToMixturesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(MixtureListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Mixtures && p.HasReadAccess)));
            NavigateToMachinesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(MachineListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Machines && p.HasReadAccess)));
            NavigateToMachineOutgoesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(MachineOutgoListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.MachineOutgoes && p.HasReadAccess)));
            NavigateToOutgoesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(OutgoListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Outgoes && p.HasReadAccess)));
            NavigateToOutgoCategoriesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(OutgoCategoryListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.OutgoCategories && p.HasReadAccess)));
            NavigateToPersonnelCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(PersonListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Personnel && p.HasReadAccess)));
            NavigateToSalaryPaymentsCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(SalaryPaymentListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.SalaryPayments && p.HasReadAccess)));
            NavigateToMachineReportCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(MachineReportPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.MachineReport && p.HasReadAccess)));
            NavigateToCompanyAggregateReportCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(CompanyAggregateReportPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.CompanyAggregateReport && p.HasReadAccess)));
            NavigateToUsersCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(UserListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin"));
            NavigateToNotesCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(listPage => NavigateCommand.Execute(typeof(NoteListPage)), this.WhenAnyValue(vm => vm.LoggedInUser).WhereNotNull().Select(u => u.Username == "admin" || u.Permissions.Any(p => p.PermissionType == PermissionTypes.Notes && p.HasReadAccess)));

            TabControlViewModel.WhenAnyValue(vm => vm.SelectedTabViewModel)
                    .Where(stvm => stvm?.RelatedMainPage is not null)
                    .Do(stvm => ListPage = stvm.RelatedMainPage)
                    .Subscribe();

            ShowCreateUpdatePageCommand = ReactiveCommand.Create<Page>(page =>
            {
                CreateUpdatePage = page;
            });

            CreateUpdatePageReturnedCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                CreateUpdatePage = null;
            });

            ShowSecondCreateUpdatePageCommand = ReactiveCommand.Create<Page>(page =>
            {
                SecondCreateUpdatePage = page;
            });

            SecondCreateUpdatePageReturnedCommand = ReactiveCommand.Create<Unit>(_ =>
            {
                SecondCreateUpdatePage = null;
            });

            InitiateSearchCommand = ReactiveCommand.Create<SearchBaseDto, SearchBaseDto>(model =>
            {
                return model;
            }, this.WhenAnyValue(vm => vm.IsLocked).Where(il => !il));

            OpenLogFileCommand = ReactiveCommand.Create<string>(_ =>
            {
                Process.Start("notepad.exe", $"C:\\{_}");
            });

            //ChangeLanguageCommand = ReactiveCommand.Create<Unit>(_ => Language = Language == AppLanguages.English ? AppLanguages.Persian : AppLanguages.English);

            BackupCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
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

            RestoreCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ =>
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

            CreateDatabaseCommand = ReactiveCommand.CreateRunInBackground<Unit>(_ => new DbContextFactory().CreateDbContext(null).Database.Migrate());

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

            this.WhenAnyValue(vm => vm.Language)
                .Do(language =>
                {
                    App.Current.Resources.MergedDictionaries.Clear();
                    App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"{language}ResourceDictionary.xaml", UriKind.Relative) });
                }).Subscribe();
        }

        [Reactive]
        public User LoggedInUser { get; set; }

        [Reactive]
        public AppLanguages Language { get; set; } = AppLanguages.English;

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
        public ReactiveCommand<Type, Unit> NavigateCommand { get; set; }
        //
        public ReactiveCommand<Unit, Unit> NavigateToBuysCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToSalesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToAccountsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToCashesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToChequesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToCompaniesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToSitesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToMaterialsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToMixturesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToMachinesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToMachineOutgoesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToOutgoesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToOutgoCategoriesCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToPersonnelCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToSalaryPaymentsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToMachineReportCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToCompanyAggregateReportCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToUsersCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateToNotesCommand { get; set; }
        //
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePageCommand { get; set; }
        public ReactiveCommand<Page, Unit> ShowSecondCreateUpdatePageCommand { get; set; }
        public ReactiveCommand<DialogDto, Unit> ShowDialogCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturnedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SecondCreateUpdatePageReturnedCommand { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearchCommand { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFileCommand { get; set; }
        //public ReactiveCommand<Unit, Unit> ChangeLanguageCommand { get; set; }
        public ReactiveCommand<Unit, Unit> BackupCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RestoreCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateDatabaseCommand { get; set; }
    }
}
