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

        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsAsync(string orderNumber)
        {
            var response = await _httpClient.GetAsync($"orderApi/GetOrderDetails/{orderNumber}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultOrderDetailDto>>();
            }
            return new List<ResultOrderDetailDto>();  // Boş liste döndür
        }
    }
}
