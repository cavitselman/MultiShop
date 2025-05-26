using MS.DtoL.BasketDtos;

namespace MS.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        }
        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            var values = await GetBasket();

            if (values == null)
            {
                values = new BasketTotalDto
                {
                    UserId = userId,
                    DiscountCode = null,
                    BasketItems = new List<BasketItemDto>()
                };
            }

            // Eğer BasketItemDto'da UserId yoksa, dinamik olarak al
            if (string.IsNullOrEmpty(basketItemDto.UserId))
            {
                basketItemDto.UserId = userId;
            }

            if (!values.BasketItems.Any(x => x.ProductId == basketItemDto.ProductId))
            {
                values.BasketItems.Add(basketItemDto);
            }

            await SaveBasket(values);
        }

        public async Task DeleteBasket(string userId)
        {
            await _httpClient.DeleteAsync("baskets");
        }

        public async Task<BasketTotalDto> GetBasket()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");
            var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
            return values;
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var basket = await GetBasket();
            if (basket == null || basket.BasketItems == null)
            {
                return false;
            }

            var itemToRemove = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (itemToRemove != null)
            {
                basket.BasketItems.Remove(itemToRemove);
                await SaveBasket(basket);
                return true;
            }

            return false;
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _httpClient.PostAsJsonAsync<BasketTotalDto>("baskets", basketTotalDto);
        }

        public async Task UpdateBasketItem(BasketItemDto item)
        {
            var basket = await GetBasket();

            if (basket == null || basket.BasketItems == null)
            {
                throw new InvalidOperationException("Basket not found.");
            }

            var existingItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
            }

            await SaveBasket(basket);
        }
    }
}
