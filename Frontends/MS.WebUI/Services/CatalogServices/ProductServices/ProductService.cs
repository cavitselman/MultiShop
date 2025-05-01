using MS.DtoL.CatalogDtos.CategoryDtos;
using MS.DtoL.CatalogDtos.ProductDtos;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using MS.WebUI.Services.CommentServices;
using Newtonsoft.Json;

namespace MS.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;

        public ProductService(HttpClient httpClient, ICommentService commentService, ICategoryService categoryService)
        {
            _httpClient = httpClient;
            _commentService = commentService;
            _categoryService = categoryService;
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
            // Get products
            var responseMessage = await _httpClient.GetAsync("products");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);

            // Get product IDs
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
            // Get product data
            var responseMessage = await _httpClient.GetAsync("products/" + id);
            var product = await responseMessage.Content.ReadFromJsonAsync<GetByIdProductDto>();

            if (product == null)
            {
                return null; // Return null if product not found
            }

            // Get comment count
            var commentCounts = await _commentService.GetCommentCountsByProductIdsAsync(new List<string> { id });

            // Add the number of comments to the product
            if (commentCounts != null && commentCounts.ContainsKey(id))
            {
                product.CommentCount = commentCounts[id];
            }
            else
            {
                product.CommentCount = 0; // If there is no comment count, assign 0
            }

            return product;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategoryAsync()
        {
            // Make API call regarding product list
            var responseMessage = await _httpClient.GetAsync("products/productlistwithcategory");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();

            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);

            // Get product IDs
            var productIds = values.Select(p => p.ProductId).ToList();

            // Get comment count
            var commentCounts = await _commentService.GetCommentCountsByProductIdsAsync(productIds);

            // Add the number of reviews for each product
            foreach (var product in values)
            {
                if (commentCounts != null && commentCounts.ContainsKey(product.ProductId))
                {
                    product.CommentCount = commentCounts[product.ProductId];
                }
                else
                {
                    product.CommentCount = 0; // If there is no comment count, assign 0
                }

                // Get category information
                var category = await _categoryService.GetCategoryByIdAsync(product.CategoryId);
                if (category != null)
                {
                    product.CategoryName = category.CategoryName; // Associate category with product
                }
            }

            return values;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategoryByCategoryIdAsync(string CategoryId)
        {
            // Pull products from specific category
            var responseMessage = await _httpClient.GetAsync("products/ProductListWithCategoryByCategoryId?id=" + CategoryId);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);

            // Get product IDs
            var productIds = values.Select(p => p.ProductId).ToList();

            // Get comment count
            var commentCounts = await _commentService.GetCommentCountsByProductIdsAsync(productIds);

            // Get category name once
            var category = await _categoryService.GetCategoryByIdAsync(CategoryId);

            foreach (var product in values)
            {
                // Add number of comments
                if (commentCounts != null && commentCounts.ContainsKey(product.ProductId))
                {
                    product.CommentCount = commentCounts[product.ProductId];
                }
                else
                {
                    product.CommentCount = 0;
                }

                // Add category name
                if (category != null)
                {
                    product.CategoryName = category.CategoryName;
                }
            }

            return values;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
