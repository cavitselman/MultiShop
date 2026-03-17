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
    [Route("Admin/Order")]
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

        // SİPARİŞ LİSTESİ
        [HttpGet("OrderList")]
        public async Task<IActionResult> OrderList()
        {
            ViewBag.v0 = "Siparişler";
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Sipariş İşlemleri";
            ViewBag.v3 = "Sipariş Listesi";

            var orders = await _orderOrderingService.GetAllOrderingAsync();
            // Listeyi tarihe göre (yeni en üstte) sıralayalım
            var model = orders.OrderByDescending(o => o.OrderDate).ToList();

            return View(model);
        }

        // SİPARİŞ DETAYLARI (KARGO BİLGİSİ DAHİL)
        [HttpGet("OrderDetailList/{id}")]
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

            // Eğer sipariş kargolandıysa, kargo detaylarını da çekiyoruz
            var cargoDetails = await _cargoDetailService.GetByOrderingIdAsync(id);

            var model = new OrderDetailViewModel
            {
                OrderingId = order.OrderingId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                OrderDetails = orderDetails,
                // Kargo detayı varsa view model'e ekleyebilirsin veya ViewBag ile taşıyabilirsin
                // CargoDetail = cargoDetails.FirstOrDefault() 
            };

            // View tarafında kargo bilgisini göstermek istersen:
            ViewBag.CargoInfo = cargoDetails.FirstOrDefault();

            return View(model);
        }

        // SİPARİŞ İPTAL ETME
        [HttpPost("Cancel/{id}")]
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
                Status = 1 // 1: İptal Edildi
            };

            await _orderOrderingService.UpdateOrderingAsync(updateDto);

            return RedirectToAction("OrderList");
        }

    }
}
