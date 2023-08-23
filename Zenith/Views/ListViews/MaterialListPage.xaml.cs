﻿using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Zenith.Models.SearchModels;
using Zenith.Models;
using Zenith.Repositories;
using Zenith.ViewModels.ListViewModels;
using Zenith.Views.CreateOrUpdateViews;
using ReactiveUI;
using System.Reactive.Linq;
using Zenith.Assets.Extensions;

namespace Zenith.Views.ListViews
{
    /// <summary>
    /// Interaction logic for MaterialListPage.xaml
    /// </summary>
    public partial class MaterialListPage : BaseListPage<Material>
    {
        public MaterialListPage()
        {
            InitializeComponent();
            var searchModel = new MaterialSearchModel();

            IObservable<Func<Material, bool>> dynamicFilter = searchModel.WhenAnyValue(s => s.Title)
                .Throttle(TimeSpan.FromMilliseconds(250)).ObserveOn(SynchronizationContext.Current)
                .Select(s => new { Title = s }).Select(s => new Func<Material, bool>(p => true));

            ViewModel = new BaseListViewModel<Material>(new MaterialRepository(), searchModel, dynamicFilter)
            {
                CreateUpdatePage = new MaterialPage()
            };

            this.WhenActivated(d => 
            {
                listItemsControl.ItemsSource = ViewModel.ActiveList;
            });
        }
    }
}
