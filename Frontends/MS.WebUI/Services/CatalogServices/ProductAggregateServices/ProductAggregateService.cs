using MS.DtoL.CatalogDtos.ProductDtos;
using MS.DtoL.CatalogDtos.ProductImageDtos;
using MS.DtoL.CatalogDtos.ProductDetailDtos;
using MS.WebUI.Services.CatalogServices.ProductDetailServices;
using MS.WebUI.Services.CatalogServices.ProductImageServices;
using MS.WebUI.Services.CatalogServices.ProductServices;

namespace MS.WebUI.Services.CatalogServices.ProductAggregateServices
{
    public class ProductAggregateService : IProductAggregateService
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDetailService _productDetailService;

        public ProductAggregateService(
            IProductService productService,
            IProductImageService productImageService,
            IProductDetailService productDetailService)
        {
            _productService = productService;
            _productImageService = productImageService;
            _productDetailService = productDetailService;
        }

        public async Task CreateProductFullAsync(CreateProductDto dto)
        {
            string productId = null;
            try
            {
                // Create product (frontend API client does not return id)
                await _productService.CreateProductAsync(dto);

                // Try to locate created product (best-effort)
                var products = await _productService.GetAllProductAsync();
                var created = products.FirstOrDefault(p =>
                    p.ProductName == dto.ProductName &&
                    p.ProductPrice == dto.ProductPrice &&
                    p.CategoryId == dto.CategoryId);

                productId = created?.ProductId;

                if (!string.IsNullOrWhiteSpace(productId))
                {
                    if (!string.IsNullOrWhiteSpace(dto.Image1) ||
                        !string.IsNullOrWhiteSpace(dto.Image2) ||
                        !string.IsNullOrWhiteSpace(dto.Image3) ||
                        !string.IsNullOrWhiteSpace(dto.Image4))
                    {
                        await _productImageService.CreateProductImageAsync(new CreateProductImageDto
                        {
                            ProductId = productId,
                            Image1 = dto.Image1,
                            Image2 = dto.Image2,
                            Image3 = dto.Image3,
                            Image4 = dto.Image4
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(dto.ProductDescription) || !string.IsNullOrWhiteSpace(dto.ProductInfo))
                    {
                        await _productDetailService.CreateProductDetailAsync(new CreateProductDetailDto
                        {
                            ProductId = productId,
                            ProductDescription = dto.ProductDescription,
                            ProductInfo = dto.ProductInfo
                        });
                    }
                }
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(productId))
                {
                    // cleanup using frontend methods: first get by productId, then delete by returned entity id
                    var img = await _productImageService.GetByProductIdProductImageAsync(productId);
                    if (img != null)
                    {
                        await _productImageService.DeleteProductImageAsync(img.ProductImageId);
                    }

                    var detail = await _productDetailService.GetByProductIdProductDetailAsync(productId);
                    if (detail != null)
                    {
                        await _productDetailService.DeleteProductDetailAsync(detail.ProductDetailId);
                    }

                    await _productService.DeleteProductAsync(productId);
                }

                throw;
            }
        }

        public async Task DeleteProductFullAsync(string productId)
        {
            var img = await _productImageService.GetByProductIdProductImageAsync(productId);
            if (img != null)
            {
                await _productImageService.DeleteProductImageAsync(img.ProductImageId);
            }

            var detail = await _productDetailService.GetByProductIdProductDetailAsync(productId);
            if (detail != null)
            {
                await _productDetailService.DeleteProductDetailAsync(detail.ProductDetailId);
            }

            await _productService.DeleteProductAsync(productId);
        }

        public async Task<List<UpdateProductFullDto>> GetAllProductFullAsync()
        {
            var products = await _productService.GetAllProductAsync();
            var result = new List<UpdateProductFullDto>();

            foreach (var p in products)
            {
                var images = await _productImageService.GetByProductIdProductImageAsync(p.ProductId);
                var detail = await _productDetailService.GetByProductIdProductDetailAsync(p.ProductId);

                result.Add(new UpdateProductFullDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    ProductImageUrl = p.ProductImageUrl,
                    CategoryId = p.CategoryId,
                    Image1 = images?.Image1,
                    Image2 = images?.Image2,
                    Image3 = images?.Image3,
                    Image4 = images?.Image4,
                    ProductDescription = detail?.ProductDescription,
                    ProductInfo = detail?.ProductInfo
                });
            }

            return result;
        }

        public async Task<UpdateProductFullDto> GetProductFullByIdAsync(string productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);
            if (product == null) return null;
            var images = await _productImageService.GetByProductIdProductImageAsync(productId);
            var detail = await _productDetailService.GetByProductIdProductDetailAsync(productId);

            return new UpdateProductFullDto
            {
                ProductId = productId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductImageUrl = product.ProductImageUrl,
                CategoryId = product.CategoryId,
                Image1 = images?.Image1,
                Image2 = images?.Image2,
                Image3 = images?.Image3,
                Image4 = images?.Image4,
                ProductDescription = detail?.ProductDescription,
                ProductInfo = detail?.ProductInfo
            };
        }

        public async Task UpdateProductFullAsync(UpdateProductFullDto dto)
        {
            await _productService.UpdateProductAsync(new UpdateProductDto
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                ProductPrice = dto.ProductPrice,
                ProductImageUrl = dto.ProductImageUrl,
                CategoryId = dto.CategoryId
            });

            var image = await _productImageService.GetByProductIdProductImageAsync(dto.ProductId);

            if (image == null)
            {
                await _productImageService.CreateProductImageAsync(new CreateProductImageDto
                {
                    ProductId = dto.ProductId,
                    Image1 = dto.Image1,
                    Image2 = dto.Image2,
                    Image3 = dto.Image3,
                    Image4 = dto.Image4
                });
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(new UpdateProductImageDto
                {
                    ProductImageId = image.ProductImageId,
                    ProductId = dto.ProductId,
                    Image1 = dto.Image1,
                    Image2 = dto.Image2,
                    Image3 = dto.Image3,
                    Image4 = dto.Image4
                });
            }

            var detail = await _productDetailService.GetByProductIdProductDetailAsync(dto.ProductId);

            if (detail == null)
            {
                await _productDetailService.CreateProductDetailAsync(new CreateProductDetailDto
                {
                    ProductId = dto.ProductId,
                    ProductDescription = dto.ProductDescription,
                    ProductInfo = dto.ProductInfo
                });
            }
            else
            {
                await _productDetailService.UpdateProductDetailAsync(new UpdateProductDetailDto
                {
                    ProductDetailId = detail.ProductDetailId,
                    ProductId = dto.ProductId,
                    ProductDescription = dto.ProductDescription,
                    ProductInfo = dto.ProductInfo
                });
            }
        }
    }
}