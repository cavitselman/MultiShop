using MS.Cargo.EL.Concrete;

namespace MS.Cargo.DAL.Abstract
{
    public interface ICargoCustomerDal : IGenericDal<CargoCustomer>
    {
        CargoCustomer GetCargoCustomerById(string id);
    }
}
