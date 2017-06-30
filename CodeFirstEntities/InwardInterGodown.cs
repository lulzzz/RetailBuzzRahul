using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardInterGodown
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string InwardCode { get; set; }
        [Required]
        public string OutwardCode { get; set; }

        [Required]
        public string GatePass { get; set; }

        public string ToGodownCode { get; set; }
        public string ToGodownName { get; set; }
        public string ToGodownContactNo { get; set; }
        public string ToGodownEmail { get; set; }

        public string EmpName { get; set; }
        public string EmpEmail { get; set; }
        public string EmpDesignation { get; set; }

        public string FromGodownCode { get; set; }
        public string FromGodownName { get; set; }
        public string FromGodownContactNo { get; set; }
        public string FromGodownEmail { get; set; }

        public string StoreKeeperName { get; set; }
        public string StoreKeeperEmail { get; set; }
        public string StoreKeeperContact { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Narration { get; set; }

    }
}
