using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
   [Serializable]
   public class CostCodeCreation
    {
       [Key]
       public int Id { get; set; }
       public DateTime Date { get; set; }
       public string Value_1 { get; set; }
       public string Value_2 { get; set; }
       public string Value_3 { get; set; }
       public string Value_4 { get; set; }
       public string Value_5 { get; set; }
       public string Value_6 { get; set; }
       public string Value_7 { get; set; }
       public string Value_8 { get; set; }
       public string Value_9 { get; set; }
       public string Value_0 { get; set; }
       public DateTime ActiveFrom { get; set; }
       public string ActiveTo { get; set; }
       public string ShopCode { get; set; }
       public string ShopName { get; set; }
       public string PreparedBy { get; set; }
       public string Status { get; set; }
       public DateTime ModifiedOn { get; set; }
    }
}
