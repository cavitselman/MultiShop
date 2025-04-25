using MS.DtoL.CatalogDtos.CategoryDtos;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CommentServices;
using Newtonsoft.Json;

namespace MS.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ICommentService _commentService;

        public ProductService(HttpClient httpClient, ICommentService commentService)
        {
            _httpClient = httpClient;
            _commentService = commentService;
        }
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync("products?id=" + id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            return values;
        }
        public async Task<List<ResultProductDto>> GetAllProductsWithCommentCountAsync()
        {
            // Ürünleri al
            var responseMessage = await _httpClient.GetAsync("products");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);

            // Ürün ID'lerini al
            var productIds = products.Select(p => p.ProductId).ToList();

            var commentCounts = await _commentService.GetCommentCountsByProductIdsAsync(productIds);

            foreach (var product in products)
            {
                if (commentCounts != null && commentCounts.ContainsKey(product.ProductId))
                {
                    product.CommentCount = commentCounts[product.ProductId];
                }
                else
                {
                    product.CommentCount = 0;
                }
            }

            return products;
        }
        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("products/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<GetByIdProductDto>();
            return values;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products/productlistwithcategory");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategoryByCategoryIdAsync(string CategoryId)
        {
            var responseMessage = await _httpClient.GetAsync("products/ProductListWithCategoryByCategoryId?id=" + CategoryId);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
