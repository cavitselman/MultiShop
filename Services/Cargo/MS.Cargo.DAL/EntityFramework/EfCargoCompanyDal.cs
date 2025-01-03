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
    public class EfCargoCompanyDal : GenericRepository<CargoCompany>, ICargoCompanyDal
    {
        public EfCargoCompanyDal(CargoContext context) : base(context)
        {
            
        }
    }
}
