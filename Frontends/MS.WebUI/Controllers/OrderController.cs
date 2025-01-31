using Microsoft.AspNetCore.Mvc;
using MS.DtoL.OrderDtos.OrderAddressDtos;
using MS.WebUI.Services.Interfaces;
using MS.WebUI.Services.OrderServices.OrderAddressServices;

namespace MS.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderAddressService _orderAddressService;
        private readonly IUserService _userService;

        public OrderController(IOrderAddressService orderAddressService, IUserService userService)
        {
            _orderAddressService = orderAddressService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Siparişler";
            ViewBag.directory3 = "Sipariş İşlemleri";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateOrderAddressDto createOrderAddressDto)
        {
            var values = await _userService.GetUserInfo();
            createOrderAddressDto.UserId = values.Id;
            await _orderAddressService.CreateOrderAddressAsync(createOrderAddressDto);
            return Redirect("/Payment/Index");
        }
    }
}
