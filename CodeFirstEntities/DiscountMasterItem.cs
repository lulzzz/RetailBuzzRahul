using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class DiscountMasterItem
    {
        [Key]
        public int Id { get; set; }
        public string DiscountCode { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public string ItemCategory { get; set; }
        public string ItemSubCategory { get; set; }
        public string ItemBrand { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Size { get; set; }
        public string Design { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public string Barcode { get; set; }
        public double? CostPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public double? DiscInPercentage { get; set; }
        public double? DiscInRupees { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
