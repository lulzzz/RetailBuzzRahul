using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardFromTailorItem
    {
        [Key]
        public int Id { get; set; }
        public string InwardCode { get; set; }
        public string Item { get; set; }
        public string Narration { get; set; }
        public double? Quantity { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
