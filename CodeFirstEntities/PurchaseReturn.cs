using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class PurchaseReturn
    {
        [Key]
        public int Id { get; set; }
        public string PurchaseReturnNo { get; set; }
        public DateTime? PurchaseReturnDate { get; set; }

        public string DebitNoteNo { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public double? DebitNoteAmount { get; set; }

        public string Code { get; set; }

        public string PoNo { get; set; }
        public string InwardNo { get; set; }

        public string SupplierBillNo { get; set; }
        public string SupplierChallanNo { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContact { get; set; }

        public double? TotalAmount { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? PackAndForwd { get; set; }
        public double? GrandTotal { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
