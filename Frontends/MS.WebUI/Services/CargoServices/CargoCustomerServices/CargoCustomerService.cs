using MS.DtoL.CargoDtos.CargoCustomerDtos;

namespace MS.WebUI.Services.CargoServices.CargoCustomerServices
{
    public class CargoCustomerService : ICargoCustomerService
    {
        private readonly HttpClient _httpClient;

        public CargoCustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<GetCargoCustomerByIdDto> GetByIdCargoCustomerInfoAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("CargoCustomers/GetCargoCustomerById?id=" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<GetCargoCustomerByIdDto>();
            return values;
        }
        public async Task InsertAsync(CreateCargoCustomerDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("CargoCustomers", dto);
            response.EnsureSuccessStatusCode();
        }

        // 3. Update by UserCustomerId
        public async Task UpdateByUserIdAsync(UpdateCargoCustomerDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync("CargoCustomers/UpdateByUserId", dto);
            response.EnsureSuccessStatusCode();
        }

        // (İsteğe Bağlı) 4. Silme işlemi
        public async Task DeleteByUserIdAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"CargoCustomers/DeleteByUserId?userId={userId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
