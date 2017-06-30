using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    public class InwardItemFromShopToGodown
    {
        [Key]
        public int InwardId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string InwardCode { get; set; }
        public string Description { get; set; }
        public double? Quantity { get; set; }
        public string Barcode { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string DesignCode { get; set; }
        public string NumberType { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string DesignName { get; set; }
        public string Unit { get; set; }
        public string ShopCode { get; set; }
        public string GodownCode { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
