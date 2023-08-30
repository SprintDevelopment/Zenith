﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Assets.Attributes;
using Zenith.Assets.Extensions;
using Zenith.Assets.Utils;
using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.ViewModels.CreateOrUpdateViewModels
{
    public class BaseCreateOrUpdateViewModel<T> : BaseViewModel<T> where T : Model, new()
    {
        public BaseCreateOrUpdateViewModel(Repository<T> repository, bool containsDeleted = false)
        {
            var modelAttributes = typeof(T).GetAttribute<ModelAttribute>();
            var modelKeyProperty = PageModel.GetKeyProperty();

            Repository = repository;

            ReturnCommand = ReactiveCommand.Create<Unit, List<T>>(_ =>
            {
                return ChangeSet;
            });

            IDisposable createOrUpdateDisposable = null;
            this.WhenAnyValue(vm => vm.PageModel).Where(pm => pm != null).Subscribe(pm =>
            {
                dynamic modelKeyId = modelKeyProperty.GetValue(pm);
                if (modelKeyProperty.PropertyType == typeof(string))
                    IsNew = string.IsNullOrWhiteSpace(modelKeyId);
                else
                    IsNew = modelKeyId == 0;

                ViewTitle = IsNew ? $"Create new {modelAttributes.SingleName}" : $"Edit {modelAttributes.SingleName}";

                if (IsNew)
                    StayOpenTitle = $"Stay open for new {modelAttributes.SingleName} ?";

                CreateOrUpdateCommand = ReactiveCommand.Create<Unit>(_ =>
                {
                    var cuCommandResult = IsNew ? Repository.Add(pm) : Repository.Update(pm, modelKeyId);
                    //Repository.SaveChanges();
                    ChangeSet.Add(Repository.Single(pm.GetKeyPropertyValue()));
                }, PageModel.ValidationContext.WhenAnyValue(context => context.IsValid));

                createOrUpdateDisposable?.Dispose();
                createOrUpdateDisposable = CreateOrUpdateCommand.Subscribe(_ =>
                {
                    if (StayOpen) // No need to modelKeyId == 0 or any extra condition because StayOpen can be true only on insert situation
                        PageModel = new T();
                    else
                        ReturnCommand.Execute().Subscribe();
                });
            });

            PrepareCommand = ReactiveCommand.Create<dynamic>(key =>
            {
                ChangeSet = new List<T>();
                if (key == null)
                    PageModel = new T();
                else
                    PageModel = MapperUtil.Mapper.Map<T>(Repository.Single(key));
            });
        }

        private ReactiveCommand<Unit, Unit> createCommand;
        public ReactiveCommand<Unit, Unit> CreateOrUpdateCommand
        {
            get { return createCommand; }
            set { this.RaiseAndSetIfChanged(ref createCommand, value); }
        }

        public ReactiveCommand<Unit, List<T>> ReturnCommand { get; set; }
        public ReactiveCommand<dynamic, Unit> PrepareCommand { get; set; }

        public List<T> ChangeSet { get; set; }

        [Reactive]
        public bool IsNew { get; set; }
        
        [Reactive]
        public bool StayOpen { get; set; }
        
        [Reactive]
        public string StayOpenTitle { get; set; }

        [Reactive]
        public T PageModel { get; set; }

        public Repository<T> Repository { get; set; }
    }
}
