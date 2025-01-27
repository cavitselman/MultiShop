using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _productService.GetProductWithCategoryByCategoryIdAsync(id);
            return View(values);
        }
    }
}
