﻿using MS.DtoL.OrderDtos.OrderAddressDtos;

namespace MS.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        //Task<List<ResultOrderAddressDto>> GetAllOrderAddressAsync();
        Task CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        //Task UpdateOrderAddressAsync(UpdateOrderAddressDto updateOrderAddressDto);
        //Task DeleteOrderAddressAsync(string id);
        //Task<UpdateOrderAddressDto> GetByIdOrderAddressAsync(string id);
    }
}
