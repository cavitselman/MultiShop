using MS.Order.Application.Features.CQRS.Results.AddressResults;
using MS.Order.Application.Interfaces;
using MS.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class GetAddressQueryHandler
    {
        private readonly IRepository<Address> _repository;
        public GetAddressQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAddressQueryResult>> Handle()
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetAddressQueryResult
            {
                AddressId = x.AddressId,
                UserId = x.UserId,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Phone = x.Phone,
                Country = x.Country,
                City = x.City,
                District = x.District,
                Detail1 = x.Detail1,
                Detail2 = x.Detail2,
                ZipCode = x.ZipCode,
                Isdefault = x.Isdefault,
                IsInvoice = x.IsInvoice
            }).ToList();
        }
    }
}
