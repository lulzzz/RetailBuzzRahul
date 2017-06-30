using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class SalesBill
    {
        [Key]
        public int Id { get; set; }
        public string SalesBillNo { get; set; }
        public DateTime? Date { get; set; }
        public string QuotationNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string DeliveryChallanNo { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientState { get; set; }
        public string ClientEmail { get; set; }
        public string ClientContactNo { get; set; }
        public string ClientType { get; set; }
        public string ConsumeResell { get; set; }
        public string FormType { get; set; }
        [Required]
        public string TransportMode { get; set; }
        [Required]
        public string TransportName { get; set; }
        public string TransportContact { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactPerson { get; set; }
        public string ShopEmail { get; set; }
        [Required]
        public string SalesPersonName { get; set; }
        public string SalesPersonAddress { get; set; }
        public string SalesPersonEmail { get; set; }
        public string SalesPersonContact { get; set; }
        public string SalesPersonDesignation { get; set; }
        public double? TotalAmount { get; set; }
        public double? PackAndForwd { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? GrandTotal { get; set; }
        public double? BillDiscount { get; set; }
        public double? PaymentReceived { get; set; }
        public double? Balance { get; set; }
        public double? Refund { get; set; }
        public double? TotalDiscount { get; set; }
        public string AdjustedStatus { get; set; }
        public double? AdjustedAmount { get; set; }
        public double? RefundToClient { get; set; }
        public string AdjustedBill { get; set; }
        public string CashierStatus { get; set; }
        [Required]
        public string PreparedBy { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SalesReturnNo { get; set; }
        public string SalesReturn { get; set; }
        public string CreditNoteNo { get; set; }
        public double? CreditNoteAmount { get; set; }
        public string TaxStatus { get; set; }
    }
}
