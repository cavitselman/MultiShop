using MS.Order.Application.Features.CQRS.Queries.AddressQueries;
using MS.Order.Application.Features.CQRS.Results.AddressResults;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;

namespace MS.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class GetAddressByIdQueryHandler
    {
        private readonly IRepository<Address> _repository;
        public GetAddressByIdQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<GetAddressByIdQueryResult> Handle(GetAddressByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
            return new GetAddressByIdQueryResult
            {
                AddressId = values.AddressId,
                UserId = values.UserId,
                Name = values.Name,
                Surname = values.Surname,
                Email = values.Email,
                Phone = values.Phone,
                Country = values.Country,
                City = values.City,
                District = values.District,
                Detail1 = values.Detail1,
                Detail2 = values.Detail2,
                ZipCode = values.ZipCode,
                Isdefault = values.Isdefault,
                IsInvoice = values.IsInvoice
            };
        }
    }
}
