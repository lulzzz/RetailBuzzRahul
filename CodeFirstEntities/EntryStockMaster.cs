using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class EntryStockMaster
    {
        [Key]
        public int EntryStockNo { get; set; }
        public string EntryStockCode { get; set; }
        [Required]
        [Display(Name = "Inward No")]
        public string PINo { get; set; }
        public string SupplierName { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public double? FinalQuantity { get; set; }
        public double? FinalAmount { get; set; }
        public DateTime? Date { get; set; }
    }
}
