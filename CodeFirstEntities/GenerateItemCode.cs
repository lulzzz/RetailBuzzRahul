using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
 public   class GenerateItemCode
    {
     [Key]
     public int id { get; set; }
     public string categoryshortname { get; set; }
     public int count { get; set; }
    }
}
