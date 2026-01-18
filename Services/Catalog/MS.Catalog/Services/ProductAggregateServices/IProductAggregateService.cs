using MS.Catalog.Dtos.ProductDtos;

namespace MS.Catalog.Services.ProductAggregateServices
{
    public interface IProductAggregateService
    {
        Task CreateProductFullAsync(CreateProductDto dto);
        Task<GetProductFullDto> GetProductFullAsync(string productId);
        Task UpdateProductFullAsync(UpdateProductFullDto dto);
        Task DeleteProductFullAsync(string productId);
        Task<UpdateProductFullDto> GetProductFullByIdAsync(string productId);
        Task<List<UpdateProductFullDto>> GetAllProductFullAsync();
    }
}
