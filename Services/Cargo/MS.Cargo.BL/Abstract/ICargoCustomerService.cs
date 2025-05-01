using MS.Cargo.EL.Concrete;

namespace MS.Cargo.BL.Abstract
{
    public interface ICargoCustomerService : IGenericService<CargoCustomer>
    {
        CargoCustomer TGetCargoCustomerById(string id);
    }
}
