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

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    ViewBag.directory1 = "Siparişler";
        //    ViewBag.directory3 = "Sipariş İşlemleri";
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Index(CreateOrderAddressDto createOrderAddressDto)
        //{
        //    var values = await _userService.GetUserInfo();
        //    createOrderAddressDto.UserId = values.Id;
        //    await _orderAddressService.CreateOrderAddressAsync(createOrderAddressDto);
        //    return Redirect("/Payment/Index");
        //}

        public async Task<IActionResult> Index(int? id, int discountRate, string discountName)
        {
            ViewBag.directory1 = "Siparişler";
            ViewBag.directory3 = "Sipariş İşlemleri";
            if (discountName != null)
            {
                ViewData["codeRateP"] = discountRate;
                ViewData["codeNameP"] = discountName;
            }
            var appUser = await _userService.GetUserInfo();
            ViewData["uId"] = appUser.Id;
            ViewData["addressId"] = id;
            return View();
        }

        public IActionResult OrderPayments()
        {
            return RedirectToAction("Index", "Payment");
        }

        public async Task<IActionResult> CreateOrderAddress(CreateOrderAddressDto createOrderAddressDto, string surname)
        {
            if (!string.IsNullOrEmpty(surname))
            {
                createOrderAddressDto.Isdefault = true;
                createOrderAddressDto.IsInvoice = true;
                if (string.IsNullOrEmpty(createOrderAddressDto.Detail2)) { createOrderAddressDto.Detail2 = "-"; }
                var appUserAddresses = await _orderAddressService.GetAddressesByUserIdAsync(createOrderAddressDto.UserId);
                var appUserDefaultAndInvoiceAddress = appUserAddresses.Where(x => x.Isdefault == true || x.IsInvoice == true).ToList();
                if (appUserDefaultAndInvoiceAddress.Count > 0)
                {
                    foreach (var item in appUserDefaultAndInvoiceAddress)
                    {
                        item.IsInvoice = false;
                        item.Isdefault = false;
                        await _orderAddressService.UpdateAddressAsync(item);
                    }
                }
                await _orderAddressService.CreateAddressAsync(createOrderAddressDto);
                return RedirectToAction("Index");
            }
            return NoContent();
        }
        public async Task<IActionResult> UpdateOrderAddress(UpdateOrderAddressDto updateOrderAddressDto, string district)
        {
            if (!string.IsNullOrEmpty(district))
            {
                bool isDefault = updateOrderAddressDto.Isdefault;
                bool isinvoice = updateOrderAddressDto.IsInvoice;
                var appUserAddresses = await _orderAddressService.GetAddressesByUserIdAsync(updateOrderAddressDto.UserId);

                if (isDefault & isinvoice)
                {
                    var appUserDefaultAndInvoiceAddress = appUserAddresses.Where(x => x.Isdefault == true || x.IsInvoice == true).ToList();
                    if (appUserDefaultAndInvoiceAddress.Count > 0)
                    {
                        foreach (var item in appUserDefaultAndInvoiceAddress)
                        {
                            item.IsInvoice = false;
                            item.Isdefault = false;
                            await _orderAddressService.UpdateAddressAsync(item);
                        }
                    }
                }
                if (!isDefault & isinvoice)
                {
                    var appUserDefaultAndInvoiceAddress = appUserAddresses.Where(x => x.IsInvoice == true).ToList();
                    if (appUserDefaultAndInvoiceAddress.Count > 0)
                    {
                        foreach (var item in appUserDefaultAndInvoiceAddress)
                        {
                            item.IsInvoice = false;
                            await _orderAddressService.UpdateAddressAsync(item);
                        }
                    }
                }
                if (isDefault & !isinvoice)
                {
                    var appUserDefaultAndInvoiceAddress = appUserAddresses.Where(x => x.Isdefault == true).ToList();
                    if (appUserDefaultAndInvoiceAddress.Count > 0)
                    {
                        foreach (var item in appUserDefaultAndInvoiceAddress)
                        {
                            item.Isdefault = false;
                            await _orderAddressService.UpdateAddressAsync(item);
                        }
                    }
                }
                await _orderAddressService.UpdateAddressAsync(updateOrderAddressDto);
                return RedirectToAction("Index", new { id = updateOrderAddressDto.AddressId });
            }
            return NoContent();
        }
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _orderAddressService.DeleteAddressAsync(id);
            return RedirectToAction("Index");
        }
    }
}
