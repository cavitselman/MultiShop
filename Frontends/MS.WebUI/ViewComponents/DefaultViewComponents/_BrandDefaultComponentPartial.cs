using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.BrandDtos;
using MS.WebUI.Services.CatalogServices.BrandServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _BrandDefaultComponentPartial : ViewComponent
    {
        private readonly IBrandService _brandService;

        public _BrandDefaultComponentPartial(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _brandService.GetAllBrandAsync();
            return View(values);
        }
    }
}
