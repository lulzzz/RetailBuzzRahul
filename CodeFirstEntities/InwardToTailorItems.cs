using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardToTailorItem
    {
        [Key]
        public int Id { get; set; }
        public string InwardCode { get; set; }
        public DateTime Date { get; set; }
        public string OutwardToTailorCode { get; set; }
        public string ClientName { get; set; }
        public string TailorName { get; set; }
        public double? TailorAmount { get; set; }
        public double? AdvancePayment { get; set; }
        public double? Balance { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
