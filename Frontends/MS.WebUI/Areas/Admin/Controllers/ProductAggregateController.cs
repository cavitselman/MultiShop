using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using MS.WebUI.Services.CatalogServices.ProductAggregateServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductAggregateController : Controller
    {
        private readonly IProductAggregateService _productAggregateService;
        private readonly ICategoryService _categoryService;

        public ProductAggregateController(IProductAggregateService productAggregateService, ICategoryService categoryService)
        {
            _productAggregateService = productAggregateService;
            _categoryService = categoryService;
        }

        void ProductViewbagList()
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";
        }

        public async Task<IActionResult> ProductListWithCategory()
        {
            ProductViewbagList();
            var values = await _productAggregateService.GetAllProductFullAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ProductViewbagList();
            var values = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> CategoryValues = values.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId
            }).ToList();
            ViewBag.CategoryValues = CategoryValues;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            await _productAggregateService.CreateProductFullAsync(dto);
            return Redirect("/Admin/ProductAggregate/ProductListWithCategory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ProductViewbagList();
            var values = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> CategoryValues = values.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId
            }).ToList();
            ViewBag.CategoryValues = CategoryValues;

            var productFull = await _productAggregateService.GetProductFullByIdAsync(id);
            return View(productFull);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductFullDto dto)
        {
            await _productAggregateService.UpdateProductFullAsync(dto);
            return Redirect("/Admin/ProductAggregate/ProductListWithCategory");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFull(string id)
        {
            await _productAggregateService.DeleteProductFullAsync(id);
            return Redirect("/Admin/ProductAggregate/ProductListWithCategory");
        }
    }
}