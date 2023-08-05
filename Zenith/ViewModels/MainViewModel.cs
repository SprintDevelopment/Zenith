using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

        public ReactiveCommand<Type, Unit> Navigate { get; set; }
        public ReactiveCommand<Page, Unit> ShowCreateUpdatePage { get; set; }
        public ReactiveCommand<DialogDto, Unit> ShowDialog { get; set; }
        public ReactiveCommand<Unit, Unit> CreateUpdatePageReturned { get; set; }
        public ReactiveCommand<SearchBaseDto, SearchBaseDto> InitiateSearch { get; set; }
        //
        public ReactiveCommand<string, Unit> OpenLogFile { get; set; }
    }
}
