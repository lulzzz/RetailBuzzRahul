using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardInterShop
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string InwardCode { get; set; }
        [Required]
        public string OutwardCode { get; set; }
        [Required]
        public string GatePass { get; set; }

        public string ToShopCode { get; set; }
        public string ToShopName { get; set; }
        public string ToShopContactNo { get; set; }
        public string ToShopEmail { get; set; }

        public string EmpName { get; set; }
        public string EmpEmail { get; set; }
        public string EmpDesignation { get; set; }

        public string FromShopCode { get; set; }
        public string FromShopName { get; set; }
        public string FromShopContactNo { get; set; }
        public string FromShopEmail { get; set; }

        public string SalesmanName { get; set; }
        public string SalesmanEmail { get; set; }
        public string SalesmanContact { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Narration { get; set; }
    }
}
