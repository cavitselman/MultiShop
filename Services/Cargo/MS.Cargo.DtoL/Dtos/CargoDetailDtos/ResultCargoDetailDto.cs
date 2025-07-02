using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Cargo.DtoL.Dtos.CargoDetailDtos
{
    public class ResultCargoDetailDto
    {
        public int CargoDetailId { get; set; }
        public string SenderCustomer { get; set; }
        public string ReceiverCustomer { get; set; }
        public string Barcode { get; set; }
        public string? Reason { get; set; }
        public string? Description { get; set; }
        public int OrderingId { get; set; }
        public int OrderDetailId { get; set; }
        public int ReturnAmount { get; set; }
        public int CargoCompanyId { get; set; }
    }
}
