using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    public class StockItemDistributuin
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string GodownCode { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
