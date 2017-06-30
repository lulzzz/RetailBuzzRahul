using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class PhysicalStockTaking
    {
        [Key]
        public int Id { get; set; }
        public string PhysicalStockTakingCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double PreviousQuantity { get; set; }
        public double CurrentQuantity { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
