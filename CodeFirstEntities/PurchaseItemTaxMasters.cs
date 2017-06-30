using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class PurchaseItemTaxMaster
    {
        [Key]
        public int TaxItemId { get; set; }
        public string TaxType { get; set; }
        public double Percentage { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
