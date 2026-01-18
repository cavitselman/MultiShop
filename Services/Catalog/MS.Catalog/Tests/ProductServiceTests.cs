using Moq;
using MS.Catalog.Dtos.ProductDetailDtos;
using MS.Catalog.Dtos.ProductDtos;
using MS.Catalog.Dtos.ProductImageDtos;
using MS.Catalog.Services.ProductAggregateServices;
using MS.Catalog.Services.ProductDetailDetailServices;
using MS.Catalog.Services.ProductImageServices;
using MS.Catalog.Services.ProductServices;
using Xunit;

namespace MS.Catalog.Tests
{
    public class ProductAggregateServiceTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IProductImageService> _mockProductImageService;
        private readonly Mock<IProductDetailService> _mockProductDetailService;
        private readonly ProductAggregateService _service;

        public ProductAggregateServiceTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockProductImageService = new Mock<IProductImageService>();
            _mockProductDetailService = new Mock<IProductDetailService>();

            _service = new ProductAggregateService(
                _mockProductService.Object,
                _mockProductImageService.Object,
                _mockProductDetailService.Object
            );
        }

        [Fact]
        public async Task CreateProductFullAsync_Should_Call_All_Services()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                ProductName = "Test Product",
                ProductPrice = 100,
                CategoryId = "6734bfdc9242e7e55a9f7d99",
                ProductDescription = "Test description",
                ProductInfo = "Test info",
                Image1 = "img1.jpg",
                Image2 = "img2.jpg",
                Image3 = "img3.jpg",
                Image4 = "img4.jpg"
            };

            // Mock product service to return a productId
            _mockProductService
                .Setup(x => x.CreateProductAndReturnIdAsync(createDto))
                .ReturnsAsync("6946940e847064d6c972e6b1");

            // Act
            await _service.CreateProductFullAsync(createDto);

            // Assert
            _mockProductImageService.Verify(x => x.CreateProductImageAsync(It.Is<CreateProductImageDto>(
    img => img.ProductId == "6946940e847064d6c972e6b1"
        && img.Image1 == createDto.Image1
        && img.Image2 == createDto.Image2
        && img.Image3 == createDto.Image3
        && img.Image4 == createDto.Image4
)), Times.Once);

            _mockProductDetailService.Verify(x => x.CreateProductDetailAsync(It.Is<CreateProductDetailDto>(
                detail => detail.ProductId == "6946940e847064d6c972e6b1"
                    && detail.ProductDescription == createDto.ProductDescription
                    && detail.ProductInfo == createDto.ProductInfo
            )), Times.Once);
        }
    }
}
