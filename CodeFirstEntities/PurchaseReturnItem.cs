using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class PurchaseReturnItem
    {
        [Key]
        public int Id { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string Barcode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public string ItemType { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public double? Quantity { get; set; }
        public double? Balance { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string DisPer { get; set; }
        public string ItemTax { get; set; }
        public double? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
