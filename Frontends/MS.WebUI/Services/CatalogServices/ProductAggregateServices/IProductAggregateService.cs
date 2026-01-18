using MS.DtoL.CatalogDtos.ProductDtos;

namespace MS.WebUI.Services.CatalogServices.ProductAggregateServices
{
    public interface IProductAggregateService
    {
        Task<List<UpdateProductFullDto>> GetAllProductFullAsync();
        Task CreateProductFullAsync(CreateProductDto dto);
        Task<UpdateProductFullDto> GetProductFullByIdAsync(string productId);
        Task UpdateProductFullAsync(UpdateProductFullDto dto);
        Task DeleteProductFullAsync(string productId);
    }
}