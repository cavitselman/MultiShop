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
            var values = await _basketService.GetBasket();

            ViewBag.code = code;
            ViewBag.discountRate = discountRate;
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Sepetim";

            // Toplam fiyat ve KDV hesaplama
            ViewBag.total = values.TotalPrice;
            ViewBag.tax = values.TotalPrice * 0.10m; // KDV %10
            ViewBag.totalPriceWithTax = values.TotalPrice + ViewBag.tax;

            // İndirim hesaplama
            if (discountRate > 0)
            {
                ViewBag.discountAmount = values.TotalPrice * (discountRate / 100m);
                ViewBag.totalNewPriceWithDiscount = ViewBag.totalPriceWithTax - ViewBag.discountAmount;
            }
            else
            {
                ViewBag.totalNewPriceWithDiscount = ViewBag.totalPriceWithTax;
            }

            // Kargo ücreti hesaplama
            ViewBag.shippingFee = ViewBag.totalNewPriceWithDiscount > 500 ? 0 : 50;
            ViewBag.shippingNote = ViewBag.shippingFee == 0 ? "500₺ ve Üzeri Kargo Bedava (Satıcı Karşılar)" : "";
            ViewBag.totalAmount = ViewBag.totalNewPriceWithDiscount + ViewBag.shippingFee;

            return View(values);
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

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {
            var basket = await _basketService.GetBasket();

            if (basket == null || basket.BasketItems == null)
            {
                return BadRequest("Sepet bulunamadı.");
            }

            var item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                await _basketService.UpdateBasketItem(item);
            }

            // Güncellenmiş değerleri hesapla
            var total = basket.BasketItems.Sum(x => x.Price * x.Quantity);
            var tax = total * 0.10m; // KDV %10
            var totalWithTax = total + tax;
            var shippingFee = totalWithTax > 500 ? 0 : 50;
            var totalAmount = totalWithTax + shippingFee;

            return Json(new
            {
                total,
                tax,
                totalWithTax,
                shippingFee,
                totalAmount
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBasketItem(string productId)
        {
            var result = await _basketService.RemoveBasketItem(productId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Ürün kaldırılırken bir hata oluştu.");
        }
    }
}
