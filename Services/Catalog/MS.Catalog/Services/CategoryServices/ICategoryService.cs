using MS.Catalog.Dtos.CategoryDtos;
using MS.Catalog.Dtos.ProductDtos;

namespace MS.Catalog.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(string id);
        Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id);
        Task<List<ResultCategoryDto>> GetCategoriesWithProductCountAsync();
    }
}
