using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CargoDtos.CargoDetailDtos;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Areas.User.Models;
using MS.WebUI.Services.CargoServices.CargoDetailServices;
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
        private readonly ICargoDetailService _cargoDetailService;

        public MyOrderController(IOrderOrderingService orderOrderingService, IUserService userService, IOrderDetailService orderDetailService, ICargoDetailService cargoDetailService)
        {
            _orderOrderingService = orderOrderingService;
            _userService = userService;
            _orderDetailService = orderDetailService;
            _cargoDetailService = cargoDetailService;
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
        public async Task<IActionResult> CancelOrder(int orderingId)
        {
            var order = await _orderOrderingService.GetByIdOrderingAsync(orderingId);

            if (order == null)
                return NotFound($"Order with id {orderingId} not found.");

            var updateDto = new UpdateOrderingDto
            {
                OrderingId = order.OrderingId,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = 3 
            };

            await _orderOrderingService.UpdateOrderingAsync(updateDto);

            return Redirect($"/User/MyOrder/MyOrderDetail/{orderingId}");
        }

        [HttpGet]
        public async Task<IActionResult> RequestRefund(int id)
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);
            if (orderDetails == null || !orderDetails.Any())
                return NotFound("İade talebi oluşturmak için ürün bulunamadı.");

            var cargoDetails = orderDetails.Select(od => new CreateCargoDetailDtoExtended
            {
                OrderingId = id,
                ProductId = od.ProductId,
                Description = "",
                Reason = "",
                IsSelected = false
            }).ToList();

            var model = new RequestRefundViewModel
            {
                OrderingId = id,
                OrderDetails = orderDetails,
                CargoDetails = cargoDetails
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestRefund(RequestRefundViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirst("sub")?.Value ?? User.Identity.Name;

            var order = await _orderOrderingService.GetByIdOrderingAsync(model.OrderingId);
            if (order == null || order.UserId != userId)
                return Unauthorized();

            var selectedItems = model.CargoDetails.Where(c => c.IsSelected).ToList();
            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "Lütfen iade edilecek en az bir ürün seçiniz.");
                return View(model);
            }

            foreach (var cargoDetail in selectedItems)
            {
                var dto = new CreateCargoDetailDto
                {
                    OrderingId = model.OrderingId,
                    ProductId = cargoDetail.ProductId,
                    Description = cargoDetail.Description,
                    Reason = cargoDetail.Reason,
                    SenderCustomer = userId,
                    ReceiverCustomer = "Admin",
                    Barcode = GenerateBarcode(),
                    CargoCompanyId = 1
                };

                await _cargoDetailService.InsertAsync(dto);
            }

            return RedirectToAction("MyOrderDetail", new { id = model.OrderingId });
        }

        private string GenerateBarcode()
        {
            var random = new Random();
            int number = random.Next(100000, 1000000);
            return $"PST{number}";
        }
    }
}
