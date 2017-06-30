using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace CodeFirstEntities
{
     [Serializable]
  public  class UnitMaster
  {
      [Key]
      public int UnitId { get; set; }
      [Required]
      [StringLength(40)]
      [Display(Name="UnitName")]
      public string UnitName { get; set; }
      public string UnitCode { get; set; }
      public string NumberType { get; set; }
      public string status { get; set; }
      public DateTime? modifiedOn { get; set; }    
    }
}
