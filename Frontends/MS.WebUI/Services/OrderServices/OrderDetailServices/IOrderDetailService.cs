using MS.DtoL.OrderDtos.OrderDetailDtos;

namespace MS.WebUI.Services.OrderServices.OrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<List<ResultOrderDetailDto>> GetOrderDetailsAsync(string orderNumber);
    }
}
