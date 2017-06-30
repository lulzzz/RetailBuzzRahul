using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class PurchaseItemDetail
    {
        [Key]
        public int PoItemNo { get; set; }
        public string PoNo { get; set; }
        public string itemCode { get; set; }
        public string UniversalBarcode { get; set; }
        public string Barcode { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public double? Quantity { get; set; }
        public string Unit { get; set; }
        public string PreviousUnit { get; set; }
        public string ConvertValue { get; set; }
        public string NumberType { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountRPS { get; set; }
        public double? Amount { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Design { get; set; }
        public string DesignName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ItemTax { get; set; }
        public string ItemType { get; set; }
        public double? ProportionateTax { get; set; }
        public double? ProportionateTaxAmt { get; set; }
    }
}
