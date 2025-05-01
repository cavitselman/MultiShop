using AutoMapper;
using MongoDB.Driver;
using MS.Catalog.Dtos.CategoryDtos;
using MS.Catalog.Entities;
using MS.Catalog.Settings;

namespace MS.Catalog.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var value = _mapper.Map<Category>(createCategoryDto);
            await _categoryCollection.InsertOneAsync(value);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(x => x.CategoryId == id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var values = await _categoryCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id)
        {
            var values = await _categoryCollection.Find<Category>(x => x.CategoryId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCategoryDto>(values);
        }

        public async Task<int> GetProductCountByCategoryIdAsync(string categoryId)
        {
            var values = await _categoryCollection.CountDocumentsAsync(x => x.CategoryId == categoryId);
            return (int)values;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var values = _mapper.Map<Category>(updateCategoryDto);
            await _categoryCollection.FindOneAndReplaceAsync(x => x.CategoryId == updateCategoryDto.CategoryId, values);
        }

        public async Task<List<ResultCategoryDto>> GetCategoriesWithProductCountAsync()
        {
            // Kategori koleksiyonunu alıyoruz
            var categories = await _categoryCollection.Find(_ => true).ToListAsync();

            // Kategoriye ait ürün sayısını hesaplamak için aggregation kullanıyoruz
            var productCountPipeline = new EmptyPipelineDefinition<Product>()
                .Group(p => p.CategoryId, g => new { CategoryId = g.Key, Count = g.Count() });

            // Ürün koleksiyonundaki verileri aggregation ile alıyoruz
            var productCounts = await _productCollection.Aggregate(productCountPipeline).ToListAsync();

            var result = new List<ResultCategoryDto>();

            // Her kategori için ürün sayısını ekliyoruz
            foreach (var category in categories)
            {
                var productCount = productCounts
                    .FirstOrDefault(p => p.CategoryId == category.CategoryId)?.Count ?? 0;

                result.Add(new ResultCategoryDto
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    ImageUrl = category.ImageUrl,
                    ProductCount = productCount
                });
            }

            return result;
        }
    }
}
