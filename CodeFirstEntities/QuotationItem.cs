using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class QuotationItem
    {
        [Key]
        public int Id { get; set; }
        public string QuotNo { get; set; }
        public string SubCategory { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Barcode { get; set; }
        public string Narration { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public string Quantity { get; set; }
        public double? Balance { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountRPS { get; set; }
        public string Amount { get; set; }
        public string ItemTax { get; set; }
        public string ItemTaxAmt { get; set; }
        public string Status { get; set; }
        public string ProportionateTax { get; set; }
        public string ProportionateTaxAmt { get; set; }
        public double? ReadOnlyShopQuantity { get; set; }
        public double? ReadOnlyTotalStockQuantity { get; set; }
        public string ProcessedIn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Attachment { get; set; }
    }
}
