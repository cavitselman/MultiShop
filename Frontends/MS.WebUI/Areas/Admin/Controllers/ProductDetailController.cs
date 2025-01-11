using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ProductDetailDtos;
using Newtonsoft.Json;
using System.Text;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class ProductDetailController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductDetailController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AddOrUpdateProductDetail(string id)
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Detayları Ekleme/Güncelleme";
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/ProductDetails/GetProductDetailByProductId?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateProductDetailDto>(jsonData);
                return View(values);
            }
            var emptyDto = new UpdateProductDetailDto { ProductId = id };
            return View(emptyDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateProductDetail(UpdateProductDetailDto updateProductDetailDto, CreateProductDetailDto createProductDetailDto, string id)
        {
            updateProductDetailDto.ProductId = id;
            createProductDetailDto.ProductId = id;
            if(!ModelState.IsValid || string.IsNullOrEmpty(updateProductDetailDto.ProductDetailId))
            {
                var client1 = _httpClientFactory.CreateClient();
                var jsonData1 = JsonConvert.SerializeObject(updateProductDetailDto);
                StringContent stringContent1 = new StringContent(jsonData1, Encoding.UTF8, "application/json");
                var responseMessage1 = await client1.PutAsync("https://localhost:7070/api/ProductDetails/", stringContent1);
                if(responseMessage1.IsSuccessStatusCode)
                {
                    return Redirect("/Admin/Product/ProductListWithCategory");
                }
                return View(createProductDetailDto);
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateProductDetailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7070/api/ProductDetails/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Redirect("/Admin/Product/ProductListWithCategory");
            }
            return View(updateProductDetailDto);
        }
    }
}
