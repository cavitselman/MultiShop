using MS.DtoL.CatalogDtos.CategoryDtos;
using Newtonsoft.Json;

namespace MS.WebUI.Services.CatalogServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCategoryDto>("categories", createCategoryDto);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _httpClient.DeleteAsync("categories?id=" + id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("categories");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
            return values;
        }

        public async Task<UpdateCategoryDto> GetByIdCategoryAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("categories/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateCategoryDto>();
            return values;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCategoryDto>("categories", updateCategoryDto);
        }

        public async Task<List<ResultCategoryDto>> GetCategoriesWithProductCountAsync()
        {
            var responseMessage = await _httpClient.GetAsync("categories/GetCategoriesWithProductCount");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultCategoryDto>();

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
            return values;
        }

        public async Task<ResultCategoryDto> GetCategoryByIdAsync(string categoryId)
        {
            var responseMessage = await _httpClient.GetAsync("categories/" + categoryId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var category = await responseMessage.Content.ReadFromJsonAsync<ResultCategoryDto>();
                return category;
            }
            return null; // Kategori bulunamazsa null döndür
        }
    }
}
