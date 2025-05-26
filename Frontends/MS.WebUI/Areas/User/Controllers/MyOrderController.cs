using Microsoft.AspNetCore.Mvc;
using MS.WebUI.Areas.User.Models;
using MS.WebUI.Services.Interfaces;
using MS.WebUI.Services.OrderServices.OrderDetailServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MS.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class MyOrderController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUserService _userService;

        public MyOrderController(IOrderOrderingService orderOrderingService, IUserService userService, IOrderDetailService orderDetailService)
        {
            _orderOrderingService = orderOrderingService;
            _userService = userService;
            _orderDetailService = orderDetailService;
        }

        public async Task<IActionResult> MyOrderList()
        {
            var user = await _userService.GetUserInfo();
            var values = await _orderOrderingService.GetOrderingByUserIdAsync(user.Id);
            var orderedValues = values.OrderByDescending(o => o.OrderDate).ToList();
            return View(orderedValues);
        }

        public async Task<IActionResult> MyOrderDetail(int id)
        {
            var order = await _orderOrderingService.GetByIdOrderingAsync(id);
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);

            if (order == null || orderDetails == null)
                return NotFound();

            var model = new OrderDetailViewModel
            {
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = orderDetails
            };

            return View(model);
        }

    }
}
