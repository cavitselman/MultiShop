using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Areas.User.Models;
using MS.WebUI.Services.CargoServices.CargoDetailServices;
using MS.WebUI.Services.OrderServices.OrderDetailServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICargoDetailService _cargoDetailService;

        public OrderController(IOrderOrderingService orderOrderingService, IOrderDetailService orderDetailService, ICargoDetailService cargoDetailService)
        {
            _orderOrderingService = orderOrderingService;
            _orderDetailService = orderDetailService;
            _cargoDetailService = cargoDetailService;
        }

        public async Task<IActionResult> OrderList()
        {
            ViewBag.v0 = "Siparişler";
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Sipariş İşlemleri";
            ViewBag.v3 = "Sipariş Listesi";

            var orders = await _orderOrderingService.GetAllOrderingAsync();

            // Eğer DTO içinde FullName yoksa sadece UserId ile bırakıyoruz.
            // İstersen burada user bilgisi de çekip model genişletebilirsin.

            var model = orders.OrderByDescending(o => o.OrderDate).ToList();

            return View(model);
        }

        public async Task<IActionResult> OrderDetailList(int id)
        {
            ViewBag.v0 = "Sipariş Detayı";
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Sipariş İşlemleri";
            ViewBag.v3 = "Sipariş Listesi";

            var order = await _orderOrderingService.GetByIdOrderingAsync(id);
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);

            if (order == null || orderDetails == null)
                return NotFound();

            var cargoDetails = await _cargoDetailService.GetByOrderingIdAsync(id);

            var model = new OrderDetailViewModel
            {
                OrderingId = order.OrderingId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                OrderDetails = orderDetails
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ship(int id)
        {
            var order = await _orderOrderingService.GetByIdOrderingAsync(id);
            if (order == null)
                return NotFound();

            var updateDto = new UpdateOrderingDto
            {
                OrderingId = order.OrderingId,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = 2 // Kargoya Verildi
            };

            await _orderOrderingService.UpdateOrderingAsync(updateDto);

            return Redirect($"/Admin/Order/OrderList");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _orderOrderingService.GetByIdOrderingAsync(id);
            if (order == null)
                return NotFound();

            var updateDto = new UpdateOrderingDto
            {
                OrderingId = order.OrderingId,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = 1 // İptal Edildi
            };

            await _orderOrderingService.UpdateOrderingAsync(updateDto);

            return Redirect($"/Admin/Order/OrderList");
        }

    }
}
