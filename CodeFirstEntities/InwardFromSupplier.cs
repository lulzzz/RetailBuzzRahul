using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardFromSupplier
    {
        [Key]
        public int InwardId { get; set; }
        public string InwardNo { get; set; }
        public DateTime? InwardDate { get; set; }
        [Required]
        public string SupplierName { get; set; }
        public string Registered { get; set; }
        public string FormType { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContactNo { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierState { get; set; }
        public string PoNo { get; set; }
        public DateTime? PoDate { get; set; }
        public DateTime? DelDate { get; set; }
        public DateTime? ReceivingDate { get; set; }
        public string ReceivedAt { get; set; }
        public string TotalReceivedQuantity { get; set; }
        public string TotalRemainingQuantity { get; set; }
        public string TotalExtraQuantity { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        [Required]
        public string ModeOfTransport { get; set; }
        [Required]
        public string TransportName { get; set; }
        public string TransportContactNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }

        public string GodownCode { get; set; }
        public string GodownName { get; set; }
        public string GodownArea { get; set; }
        public string GodownEmail { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownContactPerson { get; set; }

        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopEmail { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopContactPerson { get; set; }

        public double? TotalAmount { get; set; }
        public double? PackAndForwd { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? GrandTotal { get; set; }
        public string Narration { get; set; }
        public string Editable { get; set; }
        public string PaymentStatus { get; set; }
        public double? PaymentAmount { get; set; }
        public double? DebitNotesAmount { get; set; }
        public double? PaymentBalance { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string PurchaseReturn { get; set; }
        public double? ReadOnlyDebitNoteAmt { get; set; }
    }
}