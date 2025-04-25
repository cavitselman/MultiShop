using Microsoft.AspNetCore.Mvc;
using MS.WebUI.Services.BasketServices;

namespace MS.WebUI.ViewComponents.OrderViewComponents
{
    public class _OrderSummaryComponentPartial : ViewComponent
    {
        private readonly IBasketService _basketService;

        public _OrderSummaryComponentPartial(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string code = "", int discountRate = 0)
        {
            var basketTotal = await _basketService.GetBasket();
            var basketItems = basketTotal.BasketItems;

            // KDV hesaplama (%10)
            var totalPriceWithTax = basketTotal.TotalPrice + (basketTotal.TotalPrice * 0.10m);
            var tax = basketTotal.TotalPrice * 0.10m;

            // Eğer indirim varsa uygulanmış yeni fiyatı hesapla
            decimal totalNewPriceWithDiscount = discountRate > 0
                ? totalPriceWithTax - (totalPriceWithTax * discountRate / 100)
                : totalPriceWithTax;

            // Kargo ücreti hesaplama (500₺ üzeri ücretsiz)
            decimal shippingFee = totalNewPriceWithDiscount > 500 ? 0 : 50;

            // Genel toplam hesaplama
            decimal totalAmount = totalNewPriceWithDiscount + shippingFee;

            ViewBag.total = basketTotal.TotalPrice.ToString("N2");
            ViewBag.tax = tax.ToString("N2");
            ViewBag.totalPriceWithTax = totalPriceWithTax.ToString("N2");
            ViewBag.shippingFee = shippingFee.ToString("N2");
            ViewBag.discountRate = discountRate;
            ViewBag.totalNewPriceWithDiscount = totalNewPriceWithDiscount.ToString("N2");
            ViewBag.totalAmount = totalAmount.ToString("N2");

            return View(basketItems);
        }
    }
}
