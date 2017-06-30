using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardInterGodownItem
    {
        [Key]
        public int Id { get; set; }
        public string InwardCode { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double? Quantity { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string DesignName { get; set; }
        public string Unit { get; set; }
        public string FromGodownCode { get; set; }
        public string ToGodownCode { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
