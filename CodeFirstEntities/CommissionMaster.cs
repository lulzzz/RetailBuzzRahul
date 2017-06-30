using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class CommissionMaster
    {
        [Key]
        public int CommissionId { get; set; }
        public string CommissionCode { get; set; }
        public string EmployeeName { get; set; }
        [Required(ErrorMessage="Enter The Date")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "Enter The Date")]
        public DateTime? ToDate { get; set; }
        public string TotalSale { get; set; }
        public string TotalSaleCommPercent { get; set; }
        public string Meter { get; set; }
        public string MeterCommPercent { get; set; }
        public string Amount { get; set; }
        public string SubCategory { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCommissionPercent { get; set; }
        public string ItemCommissionRupees { get; set; }
        public string ItemSellingPrice { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
