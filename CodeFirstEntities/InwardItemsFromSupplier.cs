using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardItemsFromSupplier
    {
        [Key]
        public int InwardItemNo { get; set; }
        public string itemCode { get; set; }
        public string InwardNo { get; set; }
        public string PoNo { get; set; }
        public string Item { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Design { get; set; }
        public string DesignName { get; set; }
        public string Material { get; set; }
        public double? OrderedQuantity { get; set; }
        public double? ReceivedQuantity { get; set; }
        public double? ExtraQty { get; set; }
        public double? FreeQty { get; set; }
        public double? Balance { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? Discount { get; set; }
        public double? DiscountRPS { get; set; }
        public double? Amount { get; set; }
        public double? ProportionateTax { get; set; }
        public double? ProportionateTaxAmt { get; set; }
        public string ItemTax { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Barcode { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string PurchaseReturn { get; set; }
        public string UniversalBarcode { get; set; }
    }
}
