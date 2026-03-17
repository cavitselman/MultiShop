using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.DtoL.CargoDtos.CargoCompanyDtos;
using MS.DtoL.CargoDtos.CargoDetailDtos;
using MS.DtoL.CargoDtos.CargoOperationDtos;
using MS.DtoL.OrderDtos.OrderOrderingDtos;
using MS.WebUI.Services.CargoServices.CargoCompanyServices;
using MS.WebUI.Services.CargoServices.CargoDetailServices;
using MS.WebUI.Services.CargoServices.CargoOperationServices;
using MS.WebUI.Services.OrderServices.OrderAddressServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Cargo")]
    public class CargoController : Controller
    {
        private readonly ICargoCompanyService _cargoCompanyService;
        private readonly ICargoDetailService _cargoDetailService;
        private readonly ICargoOperationService _cargoOperationService;
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderAddressService _orderAddressService;
        private readonly IServiceScopeFactory _scopeFactory;

        public CargoController(ICargoCompanyService cargoCompanyService, ICargoDetailService cargoDetailService, ICargoOperationService cargoOperationService, IOrderOrderingService orderOrderingService, IOrderAddressService orderAddressService, IServiceScopeFactory scopeFactory)
        {
            _cargoCompanyService = cargoCompanyService;
            _cargoDetailService = cargoDetailService;
            _cargoOperationService = cargoOperationService;
            _orderOrderingService = orderOrderingService;
            _orderAddressService = orderAddressService;
            _scopeFactory = scopeFactory;
        }

        // =============================================================
        // KARGO FİRMASI İŞLEMLERİ (CRUD)
        // =============================================================

        [HttpGet("CargoCompanyList")]
        public async Task<IActionResult> CargoCompanyList()
        {
            var values = await _cargoCompanyService.GetAllCargoCompanyAsync();
            return View(values);
        }

        [HttpGet("CreateCargoCompany")]
        public IActionResult CreateCargoCompany()
        {
            return View();
        }

        [HttpPost("CreateCargoCompany")]
        public async Task<IActionResult> CreateCargoCompany(CreateCargoCompanyDto createCargoCompanyDto)
        {
            await _cargoCompanyService.CreateCargoCompanyAsync(createCargoCompanyDto);
            return RedirectToAction("CargoCompanyList");
        }

        [Route("DeleteCargoCompany/{id}")]
        public async Task<IActionResult> DeleteCargoCompany(int id)
        {
            await _cargoCompanyService.DeleteCargoCompanyAsync(id);
            return RedirectToAction("CargoCompanyList");
        }

        [HttpGet("UpdateCargoCompany/{id}")]
        public async Task<IActionResult> UpdateCargoCompany(int id)
        {
            var values = await _cargoCompanyService.GetByIdCargoCompanyAsync(id);
            return View(values);
        }

        [HttpPost("UpdateCargoCompany")]
        public async Task<IActionResult> UpdateCargoCompany(UpdateCargoCompanyDto updateCargoCompanyDto)
        {
            await _cargoCompanyService.UpdateCargoCompanyAsync(updateCargoCompanyDto);
            return RedirectToAction("CargoCompanyList");
        }

        // =============================================================
        // SİPARİŞİ KARGOYA VERME İŞLEMİ (AKILLI KISIM)
        // =============================================================

        [HttpGet("AddCargo")]
        public async Task<IActionResult> AddCargo(int id) // id: Sipariş ID
        {
            // 1. Kargo Firmaları
            var companies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            List<SelectListItem> values = (from x in companies
                                           select new SelectListItem
                                           {
                                               Text = x.CargoCompanyName,
                                               Value = x.CargoCompanyId.ToString()
                                           }).ToList();
            ViewBag.CargoCompanies = values;

            // 2. Sipariş Bilgilerini Çek (Burada UserId var)
            var orderInfo = await _orderOrderingService.GetByIdOrderingAsync(id);

            string aliciAdi = "";
            string adresBilgisi = "";

            if (orderInfo != null)
            {
                // === KISA VE ZEKİCE YOL BURASI ===
                // Yeni kod yazmak yerine var olan 'Kullanıcının Adreslerini Getir' servisini kullanıyoruz.
                // Çünkü siparişi veren kullanıcının adresi zaten sistemde kayıtlıdır.

                var userAddresses = await _orderAddressService.GetAddressesByUserIdAsync(orderInfo.UserId);

                if (userAddresses != null && userAddresses.Any())
                {
                    // Genelde son eklenen veya varsayılan adres doğrudur, ilkini alıyoruz.
                    var address = userAddresses.FirstOrDefault();

                    aliciAdi = address.Name + " " + address.Surname;
                    adresBilgisi = $"{address.District} / {address.City} - {address.Detail1}";
                }
            }

            // 3. Modeli Doldur
            var model = new CreateCargoDetailDto
            {
                OrderingId = id,
                SenderCustomer = "MultiShop Kurumsal",
                ReceiverCustomer = aliciAdi, // Otomatik doldu
                Barcode = new Random().Next(100000, 999999).ToString(),
                Description = $"Sipariş No: {orderInfo?.OrderNumber} - Adres: {adresBilgisi}" // Otomatik doldu
            };

            ViewBag.OrderingId = id;
            ViewBag.Barcode = model.Barcode;

            return View(model);
        }

        [HttpPost("AddCargo")]
        public async Task<IActionResult> AddCargo(CreateCargoDetailDto createCargoDetailDto)
        {
            // 1. Kargo Detayını Kaydet
            await _cargoDetailService.InsertAsync(createCargoDetailDto);

            // 2. İlk Hareket: Kargoya Verildi
            var operationDto = new CreateCargoOperationDto
            {
                Barcode = createCargoDetailDto.Barcode,
                Description = "Sipariş kargoya verildi.",
                OperationDate = DateTime.Now
            };
            await _cargoOperationService.InsertAsync(operationDto);

            // 3. Siparişi veritabanında 'Kargoya Verildi' (Status = 2) olarak güncelle
            var order = await _orderOrderingService.GetByIdOrderingAsync(createCargoDetailDto.OrderingId);

            if (order != null)
            {
                var updateDto = new UpdateOrderingDto
                {
                    OrderingId = order.OrderingId,
                    UserId = order.UserId,
                    OrderNumber = order.OrderNumber,
                    TotalPrice = order.TotalPrice,
                    OrderDate = order.OrderDate,
                    Status = 2 // KARGOYA VERİLDİ
                };
                await _orderOrderingService.UpdateOrderingAsync(updateDto);

                // =======================================================
                // 🔥 SİHİRLİ KISIM: HEM KARGOYU HEM ORDER'I GÜNCELLER 🔥
                // =======================================================
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 10 sn Bekle
                        await Task.Delay(TimeSpan.FromSeconds(10));

                        // Yeni bir çalışma alanı (Scope) oluşturuyoruz
                        // ARTIK _scopeFactory TANIMLI OLDUĞU İÇİN BURASI HATA VERMEYECEK
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            // A. KARGO SERVİSİNİ ÇAĞIR
                            var scopedCargoService = scope.ServiceProvider.GetRequiredService<ICargoOperationService>();

                            // B. ORDER SERVİSİNİ ÇAĞIR
                            var scopedOrderService = scope.ServiceProvider.GetRequiredService<IOrderOrderingService>();

                            // 1. Kargo Hareketine "Teslim Edildi" Ekle
                            await scopedCargoService.InsertAsync(new CreateCargoOperationDto
                            {
                                Barcode = createCargoDetailDto.Barcode,
                                Description = "Siparişiniz teslim edilmiştir.",
                                OperationDate = DateTime.Now
                            });

                            // 2. Sipariş Durumunu "Teslim Edildi" (3) Olarak GÜNCELLE
                            updateDto.Status = 3;
                            await scopedOrderService.UpdateOrderingAsync(updateDto);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Arka Plan Hatası: {ex.Message}");
                    }
                });
            }

            return Redirect("/Admin/Order/OrderList");
        }

        [HttpGet("CargoDetailList")]
        public async Task<IActionResult> CargoDetailList()
        {
            // Tüm kargo detaylarını çekiyoruz
            var values = await _cargoDetailService.GetAllAsync();
            // Listeyi tarihe göre veya ID'ye göre tersten sıralarsan son kargolar üstte çıkar
            return View(values.OrderByDescending(x => x.CargoDetailId).ToList());
        }

        [HttpGet("NewCargoOperation")]
        public IActionResult NewCargoOperation(string barcode)
        {
            ViewBag.Barcode = barcode;
            return View();
        }

        [HttpPost("NewCargoOperation")]
        public async Task<IActionResult> NewCargoOperation(CreateCargoOperationDto createCargoOperationDto)
        {
            // Tarihi otomatik şu an yapıyoruz
            createCargoOperationDto.OperationDate = DateTime.Now;

            await _cargoOperationService.InsertAsync(createCargoOperationDto);

            // İşlem bitince listeye geri dön
            return Redirect("/Admin/Cargo/CargoDetailList");
        }
    }
}
