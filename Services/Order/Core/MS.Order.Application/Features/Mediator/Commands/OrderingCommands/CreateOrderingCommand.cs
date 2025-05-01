using MediatR;

namespace MS.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class CreateOrderingCommand : IRequest
    {
        public string UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
