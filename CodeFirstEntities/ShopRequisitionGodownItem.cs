using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class ShopRequisitionGodownItem
    {
        [Key]
        public int Id { get; set; }
        public string ReqCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string StockQuantity { get; set; }
        public double? RequiredQuantity { get; set; }
        public string RequisitionToGodownName { get; set; }
        public string RequisitionToShopName { get; set; }
        public string FromShopName { get; set; }
        public string PrepareBy { get; set; }
        public string EmailIdEmployee { get; set; }
        public string ContactNoEmployee { get; set; }
        public double? GodownQuantity { get; set; }
        public double? Shopqty { get; set; }
        public DateTime? SRDate { get; set; }
        public double? RequiredQtyForGodown{get;set;}
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public IEnumerable<ShopRequisitionGodownItem> ShopoRequisitionGodownItemList { get; set; }

    }
}
