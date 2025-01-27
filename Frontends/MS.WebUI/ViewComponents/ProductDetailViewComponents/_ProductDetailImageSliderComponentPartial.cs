using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ProductImageDtos;
using MS.WebUI.Services.CatalogServices.ProductImageServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailImageSliderComponentPartial : ViewComponent
    {
        private readonly IProductImageService _productImageService;

        public _ProductDetailImageSliderComponentPartial(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _productImageService.GetByProductIdProductImageAsync(id);
            return View(values);
        }
    }
}
