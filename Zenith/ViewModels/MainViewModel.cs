using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;

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
        }

        private bool isMenuVisible;
        public bool IsMenuVisible
        {
            get { return isMenuVisible; }
            set { this.RaiseAndSetIfChanged(ref isMenuVisible, value); }
        }

        private bool isSearchVisible;
        public bool IsSearchVisible
        {
            get { return isSearchVisible; }
            set { this.RaiseAndSetIfChanged(ref isSearchVisible, value); }
        }

        private bool isLocked = false;
        public bool IsLocked
        {
            get { return isLocked; }
            set { this.RaiseAndSetIfChanged(ref isLocked, value); }
        }

        private DialogDto dialogDto;
        public DialogDto DialogDto
        {
            get { return dialogDto; }
            set { this.RaiseAndSetIfChanged(ref dialogDto, value); }
        }

        private Page createUpdatePage;
        public Page CreateUpdatePage
        {
            get { return createUpdatePage; }
            set { this.RaiseAndSetIfChanged(ref createUpdatePage, value); }
        }

        private DialogResults dialogResult;
        public DialogResults DialogResult
        {
            get { return dialogResult; }
            set { this.RaiseAndSetIfChanged(ref dialogResult, value); }
        }

        public ReactiveCommand<DialogDto, Unit> ShowDialog { get; set; }
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePage { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturned { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearch { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFile { get; set; }

    }
}
