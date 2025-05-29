using MS.DtoL.CargoDtos.CargoDetailDtos;
using Newtonsoft.Json;

namespace MS.WebUI.Services.CargoServices.CargoDetailServices
{
    public class CargoDetailService : ICargoDetailService
    {
        private readonly HttpClient _httpClient;

        public CargoDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCargoDetailDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("CargoDetails");
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ResultCargoDetailDto>>(jsonData);

            return list;
        }

        public async Task<ResultCargoDetailDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"CargoDetails/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResultCargoDetailDto>();
            return result;
        }

        public async Task InsertAsync(CreateCargoDetailDto cargoDetail)
        {
            var response = await _httpClient.PostAsJsonAsync("CargoDetails", cargoDetail);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateCargoDetailDto cargoDetail)
        {
            var response = await _httpClient.PutAsJsonAsync("CargoDetails", cargoDetail);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"CargoDetails?id={id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ResultCargoDetailDto>> GetByOrderingIdAsync(int orderingId)
        {
            var response = await _httpClient.GetAsync($"CargoDetails/ByOrderingId/{orderingId}");
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ResultCargoDetailDto>>(jsonData);

            return list;
        }

    }
}
