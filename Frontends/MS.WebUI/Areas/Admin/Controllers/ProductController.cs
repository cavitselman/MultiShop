using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.DtoL.CatalogDtos.CategoryDtos;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using MS.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        void ProductViewbagList()
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";
        }
        public async Task<IActionResult> Index()
        {
            ProductViewbagList();
            var values = await _productService.GetAllProductAsync();
            return View(values);
        }
        public async Task<IActionResult> ProductListWithCategory()
        {
            ProductViewbagList();
            var values = await _productService.GetProductWithCategoryAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ProductViewbagList();
            var values = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> CategoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId
                                                   }).ToList();
            ViewBag.CategoryValues = CategoryValues;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _productService.CreateProductAsync(createProductDto);
            return Redirect("/Admin/Product/Index");
        }

        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return Redirect("/Admin/Product/Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ProductViewbagList();
            var values = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> CategoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId
                                                   }).ToList();
            ViewBag.CategoryValues = CategoryValues;
            var productValues = await _productService.GetByIdProductAsync(id);
            return View(productValues);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return Redirect("/Admin/Product/Index");
        }        
    }
}
