using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
  [Serializable]
  public  class BloodGroup
    {
      [Key]
      public int bloodGroupID { get; set; }
      public string bloodGroupType{get;set;}
    }
}
