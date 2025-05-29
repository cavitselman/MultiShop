using MS.DtoL.CargoDtos.CargoDetailDtos;

namespace MS.WebUI.Services.CargoServices.CargoDetailServices
{
    public interface ICargoDetailService
    {
        Task<List<ResultCargoDetailDto>> GetAllAsync();
        Task<ResultCargoDetailDto> GetByIdAsync(int id);
        Task InsertAsync(CreateCargoDetailDto cargoDetail);
        Task UpdateAsync(UpdateCargoDetailDto cargoDetail);
        Task DeleteAsync(int id);
        Task<List<ResultCargoDetailDto>> GetByOrderingIdAsync(int orderingId);
    }
}
