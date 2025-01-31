using Microsoft.AspNetCore.Mvc;
using MS.WebUI.Services.BasketServices;
using MS.WebUI.Services.DiscountServices;

namespace MS.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IBasketService _basketService;

        public DiscountController(IDiscountService discountService, IBasketService basketService)
        {
            _discountService = discountService;
            _basketService = basketService;
        }

        [HttpGet]
        public PartialViewResult ConfirmDiscountCoupon()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDiscountCoupon(string code)
        {
            var values = await _discountService.GetDiscountCode(code);

            if (values == null)
            {
                // Geçersiz kupon kodu
                ViewData["InvalidCoupon"] = "Geçersiz kupon kodu.";
                return RedirectToAction("Index", "ShoppingCart", new { code = code, discountRate = 0, totalNewPriceWithDiscount = 0 });
            }

            // Geçerli kupon kodu, indirimi uygula
            var basketValues = await _basketService.GetBasket();
            var totalPriceWithTax = basketValues.TotalPrice + basketValues.TotalPrice / 100 * 10;
            var totalNewPriceWithDiscount = totalPriceWithTax - (totalPriceWithTax / 100 * values.Rate);

            return RedirectToAction("Index", "ShoppingCart", new { code = code, discountRate = values.Rate, totalNewPriceWithDiscount = totalNewPriceWithDiscount });
        }
    }
}
