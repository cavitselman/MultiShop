using MS.Cargo.DAL.Abstract;
using MS.Cargo.DAL.Concrete;
using MS.Cargo.DAL.Repositories;
using MS.Cargo.EL.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Cargo.DAL.EntityFramework
{
    public class EfCargoOperationDal : GenericRepository<CargoOperation>, ICargoOperationDal
    {
        public EfCargoOperationDal(CargoContext context) : base(context)
        {
        }
    }
}
