using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class SalesBillCreditNote
    {
        [Key]
        public int Id { get; set; }
        public string CreditNoteNo { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string BillType { get; set; }
        public string ClientName { get; set; }
        public string BillNo { get; set; }
        public string SalesReturnNo { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedBy { get; set; }
    }
}
