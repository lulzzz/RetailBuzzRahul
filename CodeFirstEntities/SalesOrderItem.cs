using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class SalesOrderItem
    {
        [Key]
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Narration { get; set; }
        public double? QuotQuantity { get; set; }
        public double? Quantity { get; set; }
        public double? SOBalance { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? Amount { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string DisInPercent { get; set; }
        public string DisInRs { get; set; }
        public string ItemTax { get; set; }
        public string ItemTaxAmt { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
        public double? ReadOnlyShopQuantity { get; set; }
        public double? ReadOnlyTotalStockQuantity { get; set; }
        public double? ReadOnlyQuotbalance { get; set; }
        public double? PropTax { get; set; }
        public double? PropTaxAmt { get; set; }
        public string QuotationNo { get; set; }
        public string ProcessedIn { get; set; }
        public string Barcode { get; set; }
    }
}
