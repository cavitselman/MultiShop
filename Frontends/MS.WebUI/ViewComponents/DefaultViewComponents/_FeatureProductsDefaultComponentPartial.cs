using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _FeatureProductsDefaultComponentPartial(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _productService.GetAllProductAsync();
            return View(values);
        }
    }
}
