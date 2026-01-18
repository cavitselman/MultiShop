using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MS.Catalog.Dtos.ProductDtos;
using MS.Catalog.Services.ProductAggregateServices;

namespace MS.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAggregateController : ControllerBase
    {
        private readonly IProductAggregateService _productAggregateService;

        public ProductAggregateController(IProductAggregateService productAggregateService)
        {
            _productAggregateService = productAggregateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductFull(CreateProductDto dto)
        {
            await _productAggregateService.CreateProductFullAsync(dto);
            return Ok("Product + Image + Detail başarıyla oluşturuldu");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductFull(string id)
        {
            var result = await _productAggregateService.GetProductFullAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductFull(UpdateProductFullDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productAggregateService.UpdateProductFullAsync(dto);
            return Ok("Product full update başarılı");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductFull(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Geçersiz ProductId");

            await _productAggregateService.DeleteProductFullAsync(id);
            return Ok("Product full delete başarılı");
        }
    }
}
