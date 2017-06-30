using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
   public class ColorCode
    {
       [Key]
       public int colorId { get; set; }
       public string colorCode { get; set; }
       public string colorName { get; set; }
       public DateTime modifiedOn { get; set; }
       public string Status { get; set; }
    }
}
