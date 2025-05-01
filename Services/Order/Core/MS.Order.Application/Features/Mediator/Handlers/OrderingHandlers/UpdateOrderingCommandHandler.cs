using MediatR;
using MS.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

namespace MS.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand>
    {
        private readonly IRepository<Ordering> _repository;

        public UpdateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderingId);
            values.OrderDate = request.OrderDate;
            values.UserId = request.UserId;
            values.TotalPrice = request.TotalPrice;
            await _repository.UpdateAsync(values);
        }
    }
}
