using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
     [Serializable]
   public class MonthExperience
    {
       [Key]
       public int MoExpId { get; set; }
       public int NoOfMonths { get; set; }
    }
}
