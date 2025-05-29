using MediatR;
using MS.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

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
            var orderNumber = await GenerateUniqueOrderNumberAsync();

            var ordering = new Ordering
            {
                OrderDate = request.OrderDate,
                TotalPrice = request.TotalPrice,
                UserId = request.UserId,
                OrderNumber = orderNumber
            };

            await _repository.CreateAsync(ordering);
        }

        private static string GenerateOrderNumber()
        {
            const string digits = "0123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var firstDigit = digits[random.Next(digits.Length)];
            var remaining = new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
            return "ORD" + firstDigit + remaining;
        }

        private async Task<string> GenerateUniqueOrderNumberAsync()
        {
            string orderNumber;
            bool exists;
            do
            {
                orderNumber = GenerateOrderNumber();
                var existing = await _repository.GetByFilterAsync(x => x.OrderNumber == orderNumber);
                exists = existing != null;
            }
            while (exists);

            return orderNumber;
        }
    }
}
