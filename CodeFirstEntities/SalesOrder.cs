using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class SalesOrder
    {
        [Key]
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public string SalesPersonName { get; set; }
        public string SalesPersonAddress { get; set; }
        public string SalesPersonEmail { get; set; }
        public string SalesPersonContact { get; set; }
        public string SalesPersonDesignation { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientState { get; set; }
        public string ClientEmail { get; set; }
        public string ClientContactNo { get; set; }
        public string ClientType { get; set; }
        public string ConsumeResell { get; set; }
        public string FormType { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactPerson { get; set; }
        public string ShopEmail { get; set; }
        [Required]
        public string TransportMode { get; set; }
        [Required]
        public string TransportName { get; set; }
        public string TransportContactNo { get; set; }
        public string QuotationNo { get; set; }
        public DateTime? QuotationDate { get; set; }
        [Required]
        public string PreparedBy { get; set; }
        public string PreparedEmail { get; set; }
        public string PreparedContact { get; set; }
        public string PreparedTime { get; set; }
        public string PackAndForwd { get; set; }
        public string TotalDiscount { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalAdvancePayment { get; set; }
        public double? AdvancePayment { get; set; }
        public double? RemainingAdvance { get; set; }
        public double? GrandTotal { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? RefundAmount { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SendingStatus { get; set; }
        public string ProcessedIn { get; set; }
        public string Editable { get; set; }
        public string CashierStatus { get; set; }
        public double? AdjustedAmount { get; set; }
        public double? RefundToClient { get; set; }
        public string AdjustedForBill { get; set; }
    }
}
