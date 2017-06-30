using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class ResetRetailBill
    {
        [Key]
        public int RetailMasterId { get; set; }
        public string RetailBillNo { get; set; }
        public string QuotationNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string DeliveryChallanNo { get; set; }
        public DateTime? Date { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string ClientEmail { get; set; }
        public string ClientContact { get; set; }
        [Required]
        public string SalesPersonName { get; set; }
        public string SalesPersonAddress { get; set; }
        public string SalesPersonEmail { get; set; }
        public string SalesPersonContact { get; set; }
        public string SalesPersonDesignation { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? GrandTotal { get; set; }
        public double? Balance { get; set; }
        public double? Refund { get; set; }
        public double? Payment { get; set; }
        public string PreparedBy { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ShopCode { get; set; }
        public string CashierStatus { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactPerson { get; set; }
        public string ShopEmail { get; set; }
        public string AdjustedStatus { get; set; }
        public double? AdjustedAmount { get; set; }
        public double? RefundToClient { get; set; }
        public string AdjustedBill { get; set; }
        public string SalesReturnNo { get; set; }
        public string SalesReturn { get; set; }
        public string BillBalance { get; set; }
        public string CreditNoteNo { get; set; }
        public double? CreditNoteAmount { get; set; }
        public string TemporaryCashMemo { get; set; }
        public string TaxStatus { get; set; }
    }
}
