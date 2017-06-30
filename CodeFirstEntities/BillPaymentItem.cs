using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class BillPaymentItem
    {
        [Key]
        public int Id { get; set; }
        public string BillPaymentCode { get; set; }
        public string InwardFromSupplierNo { get; set; }
        public DateTime? InwardFromSupplierDate { get; set; }
        public string PONo { get; set; }
        public string SupplierChallanNo { get; set; }
        public string SupplierBillNo { get; set; }
        public double? GrandTotal { get; set; }
        public double? DebitNoteAmount { get; set; }
        public double? PreviousPayment { get; set; }
        public double? Payment { get; set; }
        public double? Discount { get; set; }
        public double? Balance { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
