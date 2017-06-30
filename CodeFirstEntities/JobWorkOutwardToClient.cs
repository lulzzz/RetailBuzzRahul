using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
   public class JobWorkOutwardToClient
    {
        [Key]
        public int Id { get; set; }
        public string OutwardToTailorNo { get; set; }
        public string PaymentCode { get; set; }
        public string ClientName { get; set; }
        public string TailorName { get; set; }
        public string ItemName { get; set; }
        public double? Quantity { get; set; }
        public string Narration { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
