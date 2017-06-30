using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardFromStockDistribution
    {
        [Key]
        public int InwardId { get; set; }
        [Required]
        public string StockDistributionNo { get; set; }
        public string Code { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string GodownName { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownContactPerson { get; set; }
        public string InwardNo { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public string EmployeeDesignation { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime? InwardDate { get; set; }
        public string ShopName { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopContactPerson { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string Narration { get; set; }
    }
}
