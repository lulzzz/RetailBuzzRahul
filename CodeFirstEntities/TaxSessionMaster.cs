using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
     [Serializable]
    public class TaxSessionMaster
    {
        [Key]
        public int TaxId { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string City { get; set; }
        public string Taxcode { get; set; }
    }
}
