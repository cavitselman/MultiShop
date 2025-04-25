using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.DtoL.IdentityDtos.LoginDtos;
using MS.DtoL.OrderDtos.OrderDetailDtos;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;
using System.Net.Http;
using System.Security.Claims;

namespace MS.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;

        public PaymentController(IOrderOrderingService orderOrderingService)
        {
            _orderOrderingService = orderOrderingService;
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

        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            return View();
        }

    }
}
