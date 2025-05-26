using MS.DtoL.OrderDtos.OrderDetailDtos;

namespace MS.WebUI.Services.OrderServices.OrderDetailServices
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly HttpClient _httpClient;

        public OrderDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingIdAsync(int orderingId)
        {
            var response = await _httpClient.GetAsync($"OrderDetails/{orderingId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultOrderDetailDto>>();
            }
            return new List<ResultOrderDetailDto>();  // Boş liste döndür
        }

        public async Task CreateOrderDetailAsync(CreateOrderDetailDto dto)
        {
            await _httpClient.PostAsJsonAsync("OrderDetails", dto);
        }
    }
}
