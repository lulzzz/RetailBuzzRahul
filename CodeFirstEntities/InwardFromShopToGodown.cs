using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class InwardFromShopToGodown
    {
        [Key]
        public int InwardId { get; set; }
        [Required]
        public string OutwardNo { get; set; }
        public string InwardCode { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string GatePass { get; set; }

        public string GodownCode { get; set; }
        public string GodownName { get; set; }

        [Required]
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopEmail { get; set; }

        [Required]
        public string SalesmanName { get; set; }
        public string SalesmanDesignation { get; set; }
        public string SalesmanContactNo { get; set; }
        public string SalesmanEmail { get; set; }

        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpEmail { get; set; }

        public string DebitNoteNo { get; set; }
        public string StockReturn { get; set; }

        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Narration { get; set; }
    }
}
