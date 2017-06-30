using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class CashHandover
    {
        [Key]
        public int Id { get; set; }
        public string CashHandCode { get; set; }
        public DateTime? Date { get; set; }
        public string AccountantCode { get; set; }
        public string AccountantName { get; set; }
        public string AccountantAddress { get; set; }
        public string AccountantEmail { get; set; }
        public string AccountantContact { get; set; }
        public double? HandoverCash { get; set; }
        public double? Cash_1000 { get; set; }
        public double? Cash_1000_Amt { get; set; }
        public double? Cash_500 { get; set; }
        public double? Cash_500_Amt { get; set; }
        public double? Cash_100 { get; set; }
        public double? Cash_100_Amt { get; set; }
        public double? Cash_50 { get; set; }
        public double? Cash_50_Amt { get; set; }
        public double? Cash_20 { get; set; }
        public double? Cash_20_Amt { get; set; }
        public double? Cash_10 { get; set; }
        public double? Cash_10_Amt { get; set; }
        public double? Cash_1 { get; set; }
        public double? Cash_1_Amt { get; set; }
        public string PreparedByName { get; set; }
        public string PreparedByEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
