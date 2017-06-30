using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
     [Serializable]
    public class ShopMaster
    {
        [Key]
        public int ShopId { get; set; }
        [Required]
        public string ShopName { get; set; }
        public string ShopCode { get; set; }
        public string ShopDescription { get; set; }
        [Required]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No should be between 7 and 14")]
        public string ShopContactNo { get; set; }
        [Required]
        public string ShopAddress { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string ShopEmail { get; set; }
        [Required]
        public string EmpName { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpEmail { get; set; }
         [Required]
        public string ShortCode { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
