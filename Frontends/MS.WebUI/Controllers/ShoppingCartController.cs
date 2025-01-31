using Microsoft.AspNetCore.Mvc;
using MS.DtoL.BasketDtos;
using MS.WebUI.Services.BasketServices;
using MS.WebUI.Services.CatalogServices.ProductServices;

namespace MS.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public ShoppingCartController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index(string code, int discountRate, decimal totalNewPriceWithDiscount)
        {
            ViewBag.code = code;
            ViewBag.discountRate = discountRate;
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Sepetim";
            // Eğer kupon uygulanmadıysa, toplam fiyatı ve KDV'yi göster
            var values = await _basketService.GetBasket();
            ViewBag.total = values.TotalPrice;
            var totalPriceWithTax = values.TotalPrice + values.TotalPrice / 100 * 10;
            var tax = values.TotalPrice / 100 * 10;
            ViewBag.tax = tax;
            ViewBag.totalPriceWithTax = totalPriceWithTax;
            // Eğer kupon kodu geçerliyse, yeni fiyatı göster
            if (totalNewPriceWithDiscount > 0)
            {
                ViewBag.totalNewPriceWithDiscount = totalNewPriceWithDiscount;
            }
            else
            {
                ViewBag.totalNewPriceWithDiscount = totalPriceWithTax; // Kupon uygulanmadıysa, eski fiyatı göster
            }
            return View();
        }

        public async Task<IActionResult> AddBasketItem(string id)
        {
            var values = await _productService.GetByIdProductAsync(id);
            var items = new BasketItemDto
            {
                ProductId = values.ProductId,
                ProductName = values.ProductName,
                Price = values.ProductPrice,
                Quantity = 1,
                ProductImageUrl = values.ProductImageUrl
            };
            await _basketService.AddBasketItem(items);
            return Redirect("/ShoppingCart/Index");
        }

        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            await _basketService.RemoveBasketItem(id);
            return Redirect("/ShoppingCart/Index");
        }
    }
}
