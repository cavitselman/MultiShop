using Microsoft.AspNetCore.Mvc;
using MS.DtoL.BasketDtos;
using MS.DtoL.OrderDtos.OrderDetailDtos;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Services.BasketServices;
using MS.WebUI.Services.CatalogServices.ProductServices;
using MS.WebUI.Services.OrderServices.OrderDetailServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MS.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderDetailService _orderDetailService;

        public ShoppingCartController(IProductService productService, IBasketService basketService, IOrderOrderingService orderOrderingService, IOrderDetailService orderDetailService)
        {
            _productService = productService;
            _basketService = basketService;
            _orderOrderingService = orderOrderingService;
            _orderDetailService = orderDetailService;
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

        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            await _basketService.RemoveBasketItem(id);
            return Redirect("/ShoppingCart/Index");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder()
        {
            var basket = await _basketService.GetBasket();

            // TempData'dan indirim oranını al
            int discountRate = TempData["DiscountRate"] != null ? Convert.ToInt32(TempData["DiscountRate"]) : 0;

            decimal totalPrice = basket.TotalPrice;
            decimal tax = totalPrice * 0.10m;
            decimal totalWithTax = totalPrice + tax;
            decimal discountAmount = discountRate > 0 ? totalWithTax * (discountRate / 100m) : 0;
            decimal totalNewPriceWithDiscount = totalWithTax - discountAmount;

            var createOrderingDto = new CreateOrderingDto
            {
                UserId = basket.UserId,
                TotalPrice = totalNewPriceWithDiscount,
                OrderDate = DateTime.Now,
                OrderNumber = Guid.NewGuid().ToString().Substring(0, 8)
            };
            await _orderOrderingService.CreateOrderingAsync(createOrderingDto);

            // Sipariş detayları ekleniyor (değişiklik yok)
            var orders = await _orderOrderingService.GetOrderingByUserIdAsync(basket.UserId);
            var lastOrder = orders.OrderByDescending(x => x.OrderDate).FirstOrDefault();

            foreach (var item in basket.BasketItems)
            {
                var orderDetailDto = new CreateOrderDetailDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductPrice = item.Price,
                    ProductAmount = item.Quantity,
                    ProductTotalPrice = item.Price * item.Quantity,
                    OrderingId = lastOrder.OrderingId
                };
                await _orderDetailService.CreateOrderDetailAsync(orderDetailDto);
            }            

            return RedirectToAction("Index", "Order");
        }
    }
}
