using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.DtoL.CatalogDtos.ProductDtos
{
    public class UpdateProductFullDto
    {
        public string ProductId { get; set; }

        // Product
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImageUrl { get; set; }
        public string CategoryId { get; set; }

        // Images
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }

        // Detail
        public string ProductDescription { get; set; }
        public string ProductInfo { get; set; }
    }
}
