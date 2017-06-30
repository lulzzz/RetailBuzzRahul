using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardInterShop
    {
        [Key]
        public int OutwardId { get; set; }
        public string OutwardCode { get; set; }
        public DateTime OutwardDate { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Narration { get; set; }
        public string FromShopName { get; set; }
        public string FromShopCode { get; set; }
        public string FromShopAddress { get; set; }
        public string FromShopContactNo { get; set; }
        public string FromShopEmail { get; set; }
        public string FromShopContactPerson { get; set; }
        public string ToShopName { get; set; }
        [Required]
        public string ToShopCode { get; set; }
        public string ToDeliveryAt { get; set; }
        public string ToShopContactNo { get; set; }
        public string ToShopEmail { get; set; }
        public string ToShopContactPerson { get; set; }
        public string SalesmanName { get; set; }
        public string SalesmanEmail { get; set; }
        public string SalesmanDesignation { get; set; }
        public string SalesmanContactNo { get; set; }
        public DateTime PrepareTime { get; set; }
        public string PrepareBy { get; set; }
        public string PrepareByDesignation { get; set; }
        public string PrepareByEmail { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
