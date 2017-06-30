using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
 [Serializable]
   public class ShopRequisitionGodownsalesbill
    {
        [Key]
        public int Id { get; set; }
        public string ReqCode { get; set; }
        public DateTime? SRDate { get; set; }
        public string FromShopName { get; set; }
        public string FromShopCode { get; set; }
        //[Required]
        public string ToGodownName { get; set; }
        //public string ToGodownCode { get; set; }
        // [Required]
        public string ToShopName { get; set; }
        //public string ToShopCode { get; set; }
        public string RequestForPO { get; set; }
        public string PrepareBy { get; set; }
        public string EmailIdEmployee { get; set; }
        public string ContactNoEmployee { get; set; }
        public DateTime? PrepareTimeEmployee { get; set; }
       // public IEnumerable<ShopRequisitionGodown> ShopRequisitionGodownList { get; set; }
    }
}
