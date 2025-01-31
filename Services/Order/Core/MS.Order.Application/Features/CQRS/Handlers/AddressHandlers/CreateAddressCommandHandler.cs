using MS.Order.Application.Features.CQRS.Commands.AddressCommands;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class CreateAddressCommandHandler
    {
        private readonly IRepository<Address> _repository;
        public CreateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }
        public async Task Handle(CreateAddressCommand createAddressCommand)
        {
            await _repository.CreateAsync(new Address
            {
                City = createAddressCommand.City,
                Detail1 = createAddressCommand.Detail1,
                District = createAddressCommand.District,
                UserId = createAddressCommand.UserId,
                Country = createAddressCommand.Country,
                Detail2 = createAddressCommand.Detail2,
                Email = createAddressCommand.Email,
                Phone = createAddressCommand.Phone,
                Name = createAddressCommand.Name,
                Surname = createAddressCommand.Surname,
                ZipCode = createAddressCommand.ZipCode
            });
        }
    }
}
