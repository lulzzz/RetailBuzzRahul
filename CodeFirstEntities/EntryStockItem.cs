using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class EntryStockItem
    {
        [Key]
        public int EntryStockItemNo { get; set;}
        public string EntryStockCode { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public double? StockQuantity { get; set; }
        public double? OrderedQuantity { get; set; }
        public double? ReceivedQuantity { get; set; }
        public double? TotalQuantity { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Material { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string Color { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string Percentage { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string Barcode { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
