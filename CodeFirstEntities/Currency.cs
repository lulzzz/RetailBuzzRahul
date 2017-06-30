using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
 public   class Currency
    {
     [Key]
     public int currencyid { get; set; }
     public string currencyname { get; set; }
    }
}



