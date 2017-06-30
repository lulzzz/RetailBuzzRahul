using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class RetailBillAdjAmtDetail
    {
        [Key]
        public int Id { get; set; }
        public string RetailBillNo { get; set; }
        public string AdjustedBill { get; set; }
        public double? AdjustedAmount { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
