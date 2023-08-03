using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
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

        [Reactive]
        public Page ListPage { get; set; }

        [Reactive]
        public Page CreateUpdatePage { get; set; }

        [Reactive]
        public DialogDto DialogDto { get; set; }

        [Reactive]
        public DialogResults DialogResult { get; set; }

        public ReactiveCommand<Type, Unit> Navigate { get; set; }
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePage { get; set; }
        public ReactiveCommand<DialogDto, Unit> ShowDialog { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturned { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearch { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFile { get; set; }

    }
}
