using MS.DtoL.CargoDtos.CargoOperationDtos;

namespace MS.WebUI.Services.CargoServices.CargoOperationServices
{
    public class CargoOperationService : ICargoOperationService
    {
        private readonly HttpClient _httpClient;

        public CargoOperationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCargoOperationDto>> GetAllAsync()
        {
            // JsonConvert YERİNE ReadFromJsonAsync KULLANIYORUZ
            var response = await _httpClient.GetAsync("CargoOperations");
            return await response.Content.ReadFromJsonAsync<List<ResultCargoOperationDto>>();
        }

        public async Task<ResultCargoOperationDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"CargoOperations/{id}");
            return await response.Content.ReadFromJsonAsync<ResultCargoOperationDto>();
        }

        public async Task InsertAsync(CreateCargoOperationDto cargoOperation)
        {
            var response = await _httpClient.PostAsJsonAsync("CargoOperations", cargoOperation);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateCargoOperationDto cargoOperation)
        {
            var response = await _httpClient.PutAsJsonAsync("CargoOperations", cargoOperation);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"CargoOperations/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
