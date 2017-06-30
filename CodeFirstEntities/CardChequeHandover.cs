using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class CardChequeHandover
    {

        [Key]
        public int Id { get; set; }
        public string CardChequeHandCode { get; set; }
        public string AccountantCode { get; set; }
        public string AccountantName { get; set; }
        public string AccountantAddress { get; set; }
        public string AccountantEmail { get; set; }
        public string AccountantContact { get; set; }
        public DateTime? Date { get; set; }
        public string HandoverType { get; set; }
        public string CashierCode { get; set; }
        public string BillNo { get; set; }
        public DateTime? CardChequeDate { get; set; }
        public string CardChequeNo { get; set; }
        public double? CardChequeAmount { get; set; }
        public string PreparedByName { get; set; }
        public string PreparedByEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }



    }
}
