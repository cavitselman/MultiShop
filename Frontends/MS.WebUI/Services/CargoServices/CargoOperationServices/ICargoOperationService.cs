using MS.DtoL.CargoDtos.CargoOperationDtos;

namespace MS.WebUI.Services.CargoServices.CargoOperationServices
{
    public interface ICargoOperationService
    {
        Task<List<ResultCargoOperationDto>> GetAllAsync();
        Task InsertAsync(CreateCargoOperationDto cargoOperation);
        Task UpdateAsync(UpdateCargoOperationDto cargoOperation);
        Task DeleteAsync(int id);
        Task<ResultCargoOperationDto> GetByIdAsync(int id);
    }
}
