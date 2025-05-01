using MS.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

namespace MS.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class UpdateOrderDetailCommandHandler
    {
        private readonly IRepository<OrderDetail> _repository;
        public UpdateOrderDetailCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderDetailCommand command)
        {
            var values = await _repository.GetByIdAsync(command.OrderDetailId);
            values.OrderingId = command.OrderingId;
            values.ProductId = command.ProductId;
            values.ProductPrice = command.ProductPrice;
            values.ProductName = command.ProductName;
            values.ProductTotalPrice = command.ProductTotalPrice;
            values.ProductAmount = command.ProductAmount;
            await _repository.UpdateAsync(values);
        }
    }
}
