using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.FeatureSliderDtos;
using MS.WebUI.Services.CatalogServices.FeatureSliderServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultComponentPartial : ViewComponent
    {
        private readonly IFeatureSliderService _featureSliderService;

        public _CarouselDefaultComponentPartial(IFeatureSliderService featureSliderService)
        {
            _featureSliderService = featureSliderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureSliderService.GetAllFeatureSliderAsync();
            return View(values);
        }
    }
}
