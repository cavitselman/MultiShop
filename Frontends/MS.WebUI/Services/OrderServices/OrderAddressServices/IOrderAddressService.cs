using MS.DtoL.OrderDtos.OrderAddressDtos;

namespace MS.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        Task<List<ResultAddressesByUserDto>> GetAllAddressAsync();
        Task<List<UpdateOrderAddressDto>> GetAddressesByUserIdAsync(string id);
        Task CreateAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        Task UpdateAddressAsync(UpdateOrderAddressDto updateOrderAddressDto);
        Task DeleteAddressAsync(int id);
        Task<UpdateOrderAddressDto> GetByIdAddressAsync(int id);
    }
}
