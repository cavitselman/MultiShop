using MS.DtoL.OrderDtos.OrderDetailDtos;

namespace MS.WebUI.Areas.User.Models
{
    public class OrderDetailViewModel
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ResultOrderDetailDto> OrderDetails { get; set; }
    }
}
