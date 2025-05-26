using Microsoft.AspNetCore.Mvc;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Services.OrderServices.OrderAddressServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MS.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderAddressService _orderAddressService;

        public PaymentController(IOrderOrderingService orderOrderingService, IOrderAddressService orderAddressService)
        {
            _orderOrderingService = orderOrderingService;
            _orderAddressService = orderAddressService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Ödeme Ekranı";
            ViewBag.directory3 = "Kartla Ödeme";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateOrderingDto createOrderingDto)
        {
            await _orderOrderingService.CreateOrderingAsync(createOrderingDto);
            return Redirect("/Payment/PaymentSuccess");
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            string userId = User.FindFirst("sub")?.Value; // veya User.Identity.Name

            var order = await _orderOrderingService.GetLastOrderingByUserIdAsync(userId);

            if (order != null)
            {
                ViewBag.OrderNumber = order.OrderNumber;
                ViewBag.OrderDetails = order.OrderDetails;
                ViewBag.TotalPrice = order.TotalPrice;
            }
            else
            {
                ViewBag.OrderNumber = "Sipariş bulunamadı.";
            }

            return View();
        }

    }
}
