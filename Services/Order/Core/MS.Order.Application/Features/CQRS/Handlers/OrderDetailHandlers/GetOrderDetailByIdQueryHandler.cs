using MS.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MS.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

namespace MS.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class GetOrderDetailByIdQueryHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public GetOrderDetailByIdQueryHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderDetailByIdQueryResult>> Handle(GetOrderDetailByIdQuery query)
        {
            var orderDetails = await _repository.GetAllAsync();
            var filtered = orderDetails.Where(x => x.OrderingId == query.OrderingId).ToList();

            return filtered.Select(x => new GetOrderDetailByIdQueryResult
            {
                OrderDetailId = x.OrderDetailId,
                ProductAmount = x.ProductAmount,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                OrderingId = x.OrderingId,
                ProductPrice = x.ProductPrice,
                ProductTotalPrice = x.ProductTotalPrice
            }).ToList();
        }
    }
}
