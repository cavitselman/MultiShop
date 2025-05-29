using MS.DtoL.OrderDtos.OrderDetailDtos;
using MS.DtoL.OrderDtos.OrderOrderingDtos;

namespace MS.WebUI.Areas.User.Models
{
    public class OrderDetailViewModel
    {
        public int OrderingId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }

        public List<ResultOrderDetailDto> OrderDetails { get; set; }
    }
}
