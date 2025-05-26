using MS.DtoL.OrderDtos.OrderDetailDtos;

namespace MS.DtoL.OrderDtos.OrderOrderingDtos
{
    public class UpdateOrderingDto
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public List<ResultOrderDetailDto> OrderDetails { get; set; }
    }
}
