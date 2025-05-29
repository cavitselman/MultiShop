using MS.DtoL.CargoDtos.CargoDetailDtos;
using MS.DtoL.OrderDtos.OrderDetailDtos;

namespace MS.WebUI.Areas.User.Models
{
    public class RequestRefundViewModel
    {
        public int OrderingId { get; set; }
        public List<ResultOrderDetailDto> OrderDetails { get; set; } = new List<ResultOrderDetailDto>(); // Null olmasın diye boş listeyle başlatın
        public List<CreateCargoDetailDtoExtended> CargoDetails { get; set; } = new List<CreateCargoDetailDtoExtended>();
    }

    public class CreateCargoDetailDtoExtended : CreateCargoDetailDto
    {
        public int ReturnAmount { get; set; } = 0;
    }

}
