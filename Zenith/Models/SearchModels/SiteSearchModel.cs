﻿using ReactiveUI.Fody.Helpers;
using Zenith.Assets.Attributes;
using Zenith.Assets.Values.Dtos;
using Zenith.Assets.Values.Enums;
using Zenith.Repositories;

namespace Zenith.Models.SearchModels
{
    public class SiteSearchModel : SearchBaseDto
    {
        [Search(Title = "شرکت مربوطه", ControlType = SearchItemControlTypes.ComboBox, ValueSourceType = typeof(CompanyRepository))]
        [Reactive]
        public short CompanyId { get; set; }
    }
}