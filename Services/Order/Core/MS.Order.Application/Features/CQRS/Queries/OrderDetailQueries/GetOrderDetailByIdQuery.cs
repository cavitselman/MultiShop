namespace MS.Order.Application.Features.CQRS.Queries.OrderDetailQueries
{
    public class GetOrderDetailByIdQuery
    {
        public int OrderingId { get; set; }

        public GetOrderDetailByIdQuery(int orderingId)
        {
            OrderingId = orderingId;
        }
    }
}
