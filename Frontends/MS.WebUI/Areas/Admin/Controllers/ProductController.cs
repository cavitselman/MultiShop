using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Catalog.Dtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using MS.Catalog.Services.ProductAggregateServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductAggregateService _productAggregateService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IProductAggregateService productAggregateService,
            ICategoryService categoryService)
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
            // Eğer Product + Category listesi istiyorsak burada servis çağrısı eklenebilir
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ProductViewbagList();

            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryValues = categories
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId
                }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.CategoryValues = categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryId
                    }).ToList();

                return View(createProductDto);
            }

            // Aggregate service çağrısı
            await _productAggregateService.CreateProductFullAsync(createProductDto);

            return Redirect("/Admin/Product/ProductListWithCategory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ProductViewbagList();

            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryValues = categories
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId
                }).ToList();

            // AggregateService veya ProductService üzerinden mevcut product verilerini çek
            var product = await _productAggregateService.GetProductFullByIdAsync(id);
            // product tipinin UpdateProductFullDto olduğundan emin ol

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductFullDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.CategoryValues = categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryId
                    }).ToList();

                return View(updateProductDto);
            }

            await _productAggregateService.UpdateProductFullAsync(updateProductDto);

            return Redirect("/Admin/Product/ProductListWithCategory");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFull(string id)
        {
            await _productAggregateService.DeleteProductFullAsync(id);
            return Redirect("/Admin/Product/ProductListWithCategory");
        }
    }
}
