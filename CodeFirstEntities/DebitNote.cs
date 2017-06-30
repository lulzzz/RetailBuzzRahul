using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CodeFirstEntities
{
    [Serializable]
    public class DebitNote
    {
        [Key]
        public int Id { get; set; }
        public string DebitNoteNo { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public string SupplierName { get; set; }
        public string BillNo { get; set; }
        public string ChallanNo { get; set; }
        public string InwardNo { get; set; }
        public string PurchaseReturnNo { get; set; }
        public double? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
