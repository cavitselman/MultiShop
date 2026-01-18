using MS.Catalog.Dtos.ProductDtos;
using MS.Catalog.Dtos.ProductImageDtos;
using MS.Catalog.Dtos.ProductDetailDtos;
using MS.Catalog.Services.ProductDetailDetailServices;
using MS.Catalog.Services.ProductImageServices;
using MS.Catalog.Services.ProductServices;

namespace MS.Catalog.Services.ProductAggregateServices
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
                productId = await _productService.CreateProductAndReturnIdAsync(dto);

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

                if (!string.IsNullOrWhiteSpace(dto.ProductDescription))
                {
                    await _productDetailService.CreateProductDetailAsync(
                        new CreateProductDetailDto
                        {
                            ProductId = productId,
                            ProductDescription = dto.ProductDescription,
                            ProductInfo = dto.ProductInfo
                        });
                }
            }
            catch
            {
                if (productId != null)
                {
                    await _productImageService.DeleteByProductIdAsync(productId);
                    await _productDetailService.DeleteByProductIdAsync(productId);
                    await _productService.DeleteProductAsync(productId);
                }

                throw;
            }
        }

        public async Task DeleteProductFullAsync(string productId)
        {
            await _productImageService.DeleteByProductIdAsync(productId);
            await _productDetailService.DeleteByProductIdAsync(productId);

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

        public async Task<GetProductFullDto> GetProductFullAsync(string productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);
            if (product == null)
                return null;

            var image = await _productImageService.GetByProductIdProductImageAsync(productId);
            var detail = await _productDetailService.GetByProductIdProductDetailAsync(productId);

            return new GetProductFullDto
            {
                // Product
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductImageUrl = product.ProductImageUrl,
                CategoryId = product.CategoryId,

                // Images (null-safe)
                Image1 = image?.Image1,
                Image2 = image?.Image2,
                Image3 = image?.Image3,
                Image4 = image?.Image4,

                // Detail
                ProductDescription = detail?.ProductDescription,
                ProductInfo = detail?.ProductInfo
            };
        }

        public async Task<UpdateProductFullDto> GetProductFullByIdAsync(string productId)
        {
            // Product'u al
            var product = await _productService.GetByIdProductAsync(productId);

            // ProductImage'i al
            var images = await _productImageService.GetByProductIdProductImageAsync(productId);

            // ProductDetail'i al
            var detail = await _productDetailService.GetByProductIdProductDetailAsync(productId);

            // DTO'ya map et
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
