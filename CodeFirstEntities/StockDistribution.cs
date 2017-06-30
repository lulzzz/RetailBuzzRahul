using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class StockDistribution
    {
        [Key]
        public int Id { get; set; }
        public string StockDistributionCode { get; set; }
        public string Code { get; set; }
        public string GodownName { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownContactPerson { get; set; }
        public string ShopName { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopContactPerson { get; set; }
        public string Narration { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
