using MS.Order.Domain.Enums;

namespace MS.Order.Domain.Entities
{
    public class Ordering
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;        
    }
}
