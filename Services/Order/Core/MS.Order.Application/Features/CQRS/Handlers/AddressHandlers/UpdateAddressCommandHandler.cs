using MS.Order.Application.Features.CQRS.Commands.AddressCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

namespace MS.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class UpdateAddressCommandHandler
    {
        private readonly IRepository<Address> _repository;
        public UpdateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateAddressCommand command)
        {
            var values = await _repository.GetByIdAsync(command.AddressId);
            values.UserId = command.UserId;
            values.Name = command.Name;
            values.Surname = command.Surname;
            values.Email = command.Email;
            values.Phone = command.Phone;
            values.Country = command.Country;
            values.City = command.City;
            values.District = command.District;
            values.City = command.City;
            values.UserId = command.UserId;
            await _repository.UpdateAsync(values);
        }
    }
}
