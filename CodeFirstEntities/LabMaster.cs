using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
 public   class LabMaster
    {

     [Key]
     public int labId { get; set; }
     [Required]
     [StringLength(50)]
     [Display (Name="labName")]
     public string labName { get; set; }
     [StringLength(250)]
     [Display(Name = "labDescription")]
     public string labDescription { get; set; }
     public string logo { get; set; }
     public string labStatus { get; set; }
     public string labCode { get; set; }
     public DateTime modifiedOn { get; set; }
     public IEnumerable<LabMaster> lablists { get; set; }    
    }
}
