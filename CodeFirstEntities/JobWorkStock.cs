using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class JobWorkStock
    {
        [Key]
        public int Id { get; set; }
        public DateTime StockDate { get; set; }
        public string InwardCode { get; set; }
        public string OutwardCode { get; set; }
        public string ItemName { get; set; }
        public double? Quantity { get; set; }
        public string Narration { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
