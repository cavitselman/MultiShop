using MS.DtoL.CatalogDtos.ProductDetailDtos;

namespace MS.WebUI.Services.CatalogServices.ProductDetailServices
{
    public interface IProductDetailService
    {
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();
        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);
        Task DeleteProductDetailAsync(string id);
        Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id);
        Task<GetByIdProductDetailDto> GetByProductIdProductDetailAsync(string id);
    }
}
