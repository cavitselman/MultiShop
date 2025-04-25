using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.DtoL.OrderDtos.OrderOrderingDtos
{
    public class ResultAllOrderingListDto
    {
        public int OrderingId { get; set; }
        public string NameSurname { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
