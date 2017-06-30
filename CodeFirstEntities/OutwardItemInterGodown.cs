using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardItemInterGodown
    {
        [Key]
        public int ItemId { get; set; }
        public string Barcode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string QuantityTransfer { get; set; }
        public string TypeOfMaterial { get; set; }
        public string ColorCode { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public double? SellingPrice { get; set; }
        public double? MRP { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string OutwardCode { get; set; }
        public string Balance { get; set; }
        public string FromGdCode { get; set; }
        public string ToGdCode { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
