using Microsoft.EntityFrameworkCore;
using MS.Cargo.DAL.Concrete;
using MS.Cargo.EL.Concrete;
using MS.Order.Domain.Enums;
using MS.Order.Persistence.Context;

namespace MS.WebUI.Services.BackgroundServices
{
    public class AutoOrderDeliveryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private const int DeliveryThresholdMinutes = 2; // Test için 2 dakika

        public AutoOrderDeliveryService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Her iki Context'i de scope üzerinden alıyoruz
                        var orderDbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                        var cargoDbContext = scope.ServiceProvider.GetRequiredService<CargoContext>();

                        // 1. Durumu 'Shipped' olan siparişleri getir
                        var shippedOrders = await orderDbContext.Orderings
                            .Where(x => x.Status == OrderStatus.Shipped)
                            .ToListAsync(stoppingToken);

                        foreach (var order in shippedOrders)
                        {
                            TimeSpan timePassed = DateTime.Now - order.OrderDate;

                            if (timePassed.TotalMinutes >= DeliveryThresholdMinutes)
                            {
                                // A. Sipariş Tablosunu Güncelle
                                order.Status = OrderStatus.Delivered;

                                // B. Kargo Hareketlerine Yeni Kayıt Ekle
                                var cargoDetail = await cargoDbContext.CargoDetails
                                    .FirstOrDefaultAsync(x => x.OrderingId == order.OrderingId, stoppingToken);

                                if (cargoDetail != null)
                                {
                                    var deliveryLog = new CargoOperation
                                    {
                                        Barcode = cargoDetail.Barcode,
                                        Description = "Siparişiniz teslim edilmiştir.",
                                        OperationDate = DateTime.Now
                                    };
                                    cargoDbContext.CargoOperations.Add(deliveryLog);
                                }
                            }
                        }

                        // Değişiklik varsa her iki DB'yi de kaydet
                        if (orderDbContext.ChangeTracker.HasChanges() || cargoDbContext.ChangeTracker.HasChanges())
                        {
                            await orderDbContext.SaveChangesAsync(stoppingToken);
                            await cargoDbContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda loglama yapılabilir
                    Console.WriteLine($"Arka plan servisi hatası: {ex.Message}");
                }

                // 1 dakika bekle ve tekrar kontrol et
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
