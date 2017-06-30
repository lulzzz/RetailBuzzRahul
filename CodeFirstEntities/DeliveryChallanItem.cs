using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class DeliveryChallanItem
    {
        [Key]
        public int Id { get; set; }
        public string ChallanNo { get; set; }
        public string QuotOrOrderNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Narration { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Design { get; set; }
        public string DesignName { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public string Quantity { get; set; }
        public string QuotOrderQty { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string Amount { get; set; }
        public string ItemTax { get; set; }
        public string ItemTaxAmt { get; set; }
        public double? DelBalance { get; set; }
        public double? ExtraQty { get; set; }
        public double? DiscountRPS { get; set; }
        public double? DiscountPercent { get; set; }
        public string ProportionateTax { get; set; }
        public string ProportionateTaxAmt { get; set; }
        public double? ReadOnlyShopQuantity { get; set; }
        public double? ReadOnlyTotalStockQuantity { get; set; }
        public double? ReadOnlySOBalance { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Barcode { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        
    }
}
