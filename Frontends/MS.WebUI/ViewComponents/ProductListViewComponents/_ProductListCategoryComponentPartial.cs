using Microsoft.AspNetCore.Mvc;
using MS.WebUI.Services.CatalogServices.ProductServices;

namespace MS.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListCategoryComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListCategoryComponentPartial(IProductService productService)
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
