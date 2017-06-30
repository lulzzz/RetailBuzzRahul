using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class OutwardToShop
    {
        [Key]
        public int OutwardId { get; set; }
        public string OutwardCode { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string GatePass { get; set; }
        public string Narration { get; set; }
        [Required]
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpContactNo { get; set; }
        public string EmpEmail { get; set; }
        [Required]
        public string SalesmanName { get; set; }
        public string SalesmanDesignation { get; set; }
        public string SalesmanContactNo { get; set; }
        public string SalesmanEmail { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string PrepaidBy { get; set; }
        [Required]
        public string ShopName { get; set; }
        public string ShopCode { get; set; }
        public string ShopAddress { get; set; }
        public string ShopContactNo { get; set; }
        public string ShopEmail { get; set; }
        public string GodownCode { get; set; }
        public string GodownName { get; set; }
    }
}
