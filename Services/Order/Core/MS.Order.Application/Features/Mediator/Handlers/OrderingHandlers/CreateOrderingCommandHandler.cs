using MediatR;
using MS.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand>
    {
        private readonly IRepository<Ordering> _repository;

        public CreateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
        {
            var orderNumber = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss"); // Sipariş numarası oluşturuluyor
            var ordering = new Ordering
            {
                OrderDate = request.OrderDate,
                TotalPrice = request.TotalPrice,
                UserId = request.UserId,
                OrderNumber = orderNumber // OrderNumber ekleniyor
            };

            await _repository.CreateAsync(ordering);
        }
    }
}
