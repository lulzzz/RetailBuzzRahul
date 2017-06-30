using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class GodownMaster
    {
        [Key]
        public int GodownId { get; set; }
        [Required]
        public string GdCode { get; set; }
        [Required]
        public string GodownName { get; set; }
        [Required]
        public string ComponyName{get; set;}
        [Required]
        public string EmpName{get; set;}
        [Required]
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No should be between 7 and 14")]
        public string ContactNo1 { get; set; }
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No should be between 7 and 14")]
        public string ContactNo2 { get; set; }
        [Range(1000000, 99999999999999, ErrorMessage = "Contact No should be between 7 and 14")]
        public string ContactNo3 { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter correct email")]
        public string GodownEmail { get; set; }
        public string Designation { get; set; }
        public string EmpEmail { get; set; }
        [Required]
        public string ShortCode { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; }
    }
}
