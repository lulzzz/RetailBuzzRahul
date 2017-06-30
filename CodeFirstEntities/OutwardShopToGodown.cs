using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardShopToGodown
    {
        [Key]
        public int OutwardId { get; set; }
        public string OutwardCode { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Narration { get; set; }
        
        public string StoreKeeperName { get; set; }
        public string StoreKeeperDesignation { get; set; }
        public string StoreKeeperContactNo { get; set; }
        public string StoreKeeperEmail { get; set; }
        [Required]
        public string SalesmanName { get; set; }
        public string SalesmanDesignation { get; set; }
        public string SalesmanContactNo { get; set; }
        public string SalesmanEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string PreparedBy { get; set; }
        public string PreparedEmail { get; set; }
        public string PreparedDesignation { get; set; }
        [Required]
        public string GodownCode { get; set; }
        public string GodownName { get; set; }
        public string GodownAddress { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownEmail { get; set; }

        public string ShopCode { get; set; }
        public string ShopName { get; set; }

        public string DebitNoteNo { get; set; }
        public string StockReturn { get; set; }
    }
}
