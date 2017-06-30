using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardStockDistribution
    {
        [Key]
        public int Id { get; set; }
        public string OutwardCode { get; set; }
        public string Code { get; set; }
        public string GodownName { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownContactPerson { get; set; }
        public string ShopName { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopContactPerson { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Narration { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
