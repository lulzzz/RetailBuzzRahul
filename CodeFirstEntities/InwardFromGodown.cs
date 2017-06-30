using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardFromGodown
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OutwardNo { get; set; }
        public string InwardCode { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string GodownCode { get; set; }
        public string GodownName { get; set; }
        public string GodownEmail { get; set; }
        public string GodownContactPerson { get; set; }
        public string GodownNumber { get; set; }
        [Required]
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpEmail { get; set; }

        public string Narration { get; set; }
    }
}
