using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardInterGodown
    {
        [Key]
        public int OutwardId { get; set; }
        public string OutwardCode { get; set; }
        public DateTime? OutwardDate { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Narration { get; set; }
        [Required]
        public string FromGodownname { get; set; }
        public string FromGodownCode { get; set; }
        public string FromGodownContactNo { get; set; }
        public string FromGodownEmail { get; set; }
        public string FromGodownContactPerson { get; set; }
        public string ToGodownCode { get; set; }

        [Required(ErrorMessage = "Store Keeper Name Is Required")]
        public string FromGodownStoreKeeperName { get; set; }
        public string FromGodownStoreKeeperDesignation { get; set; }
        public string FromGodownStoreKeeperContactNo { get; set; }
        public string FromGodownStoreKeeperEmail { get; set; }

        [Required]
        public string ToGodownname { get; set; }
        public string ToDeliveryAt { get; set; }
        public string ToGodownContactNo { get; set; }
        public string ToGodownEmail { get; set; }
        public string ToGodownContactPerson { get; set; }
        [Required]
        public string PrepareBy { get; set; }
        public string PrepareByEmail { get; set; }
        public DateTime? PrepareTime { get; set; }
        public String PrepareByContactNo { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
