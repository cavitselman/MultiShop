using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailFeatureComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductDetailFeatureComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _productService.GetByIdProductAsync(id);
            return View(values);
        }
    }
}
