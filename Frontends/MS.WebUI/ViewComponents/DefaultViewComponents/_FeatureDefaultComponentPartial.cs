﻿using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.FeatureDtos;
using MS.WebUI.Services.CatalogServices.FeatureServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureDefaultComponentPartial : ViewComponent
    {
        private readonly IFeatureService _featureService;

        public _FeatureDefaultComponentPartial(IFeatureService featureService)
        {
            _featureService = featureService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureService.GetAllFeatureAsync();
            return View(values);
        }
    }
}
