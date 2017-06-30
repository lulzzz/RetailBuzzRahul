using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
     [Serializable]
    public class SalesIncentiveMaster
    {
        [Key]
        public int SIId { get; set; }
        [Required]
        public string SISlabFrom { get; set; }
        [Required]
        public string SISlabTo { get; set; }
        [Required]
        public string Percentage { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
