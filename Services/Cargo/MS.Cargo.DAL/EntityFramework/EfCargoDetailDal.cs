using MS.Cargo.DAL.Abstract;
using MS.Cargo.DAL.Concrete;
using MS.Cargo.DAL.Repositories;
using MS.Cargo.EL.Concrete;

namespace MS.Cargo.DAL.EntityFramework
{
    public class EfCargoDetailDal : GenericRepository<CargoDetail>, ICargoDetailDal
    {
        public EfCargoDetailDal(CargoContext context) : base(context)
        {
        }
    }
}
