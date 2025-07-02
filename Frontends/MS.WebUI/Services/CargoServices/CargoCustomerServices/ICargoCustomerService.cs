using MS.DtoL.CargoDtos.CargoCustomerDtos;

namespace MS.WebUI.Services.CargoServices.CargoCustomerServices
{
    public interface ICargoCustomerService
    {
        Task<GetCargoCustomerByIdDto> GetByIdCargoCustomerInfoAsync(string id);
        Task InsertAsync(CreateCargoCustomerDto dto);
        Task UpdateByUserIdAsync(UpdateCargoCustomerDto dto);
        Task DeleteByUserIdAsync(string userId);
    }
}
