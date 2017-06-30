using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardItemToClient
    {
        [Key]
        public int ItemId { get; set; }
        public string OutwardCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public string DesignName { get; set; }
        public string Unit { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? StockQuantity { get; set; }
        public double? OutwardQuantity { get; set; }
        public double? CurrentBalance { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
