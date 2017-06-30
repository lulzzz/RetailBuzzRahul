using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class SalesReturn
    {
        [Key]
        public int Id { get; set; }
        public string SalesReturnNo { get; set; }
        public string BillNo { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ClientName { get; set; }
        public string ClientContact { get; set; }
        public string CreditNoteNo { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public double? CreditNoteAmount { get; set; }
        public DateTime? SalesReturnDate { get; set; }
        public double? OldGrandTotal { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? TotalDiscount { get; set; }
        public double? PackAndForwd { get; set; }
        public double? Payment { get; set; }
        public double? GrandTotal { get; set; }
        public double? BillDiscount { get; set; }
        public double? Balance { get; set; }
        public double? RefundToClient { get; set; }
        public double? Refund { get; set; }
        public double? AdjustedAmount { get; set; }
        public string AdjustedBill { get; set; }
        public string CashierStatus { get; set; }
        public string BalanceStatus { get; set; }
        public double? RefundAmount { get; set; }
        public double? AdditionalAdvance { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string BillBalance { get; set; }
        public string TaxStatus { get; set; }
    }
}
