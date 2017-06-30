using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardToTailor
    {
        [Key]
        public int Id { get; set; }
        public string OutwardCode { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string TailorName { get; set; }
        public string TailorAddress { get; set; }
        public string TailorContact { get; set; }
        public string TailorEmail { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientContact { get; set; }
        public string ClientEmail { get; set; }
        public string RetailBills { get; set; }
        public string SalesBills { get; set; }
        public double? GrandTotal { get; set; }
        [Required]
        public double? AdvancePayment { get; set; }
        public double? PaidAmountToTailor { get; set; }
        public double? Balance { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string PreparedBy { get; set; }
        public string InwardStatus { get; set; }
        public string TailorInwardStatus { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
