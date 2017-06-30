using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CodeFirstEntities
{
    [Serializable]
   public class RequisitionOfPo
    {
       [Key]
       public int Id { get; set; }
       public string RequisitionFrom { get; set; }
       public string ItemCode { get; set; }
       public string ItemName { get; set; }
       public string Description { get; set; }
       public string QuantityInStock { get; set; }
       public string RequiredQuantity { get; set; }
       public string RejectQuantity { get; set; }
       public string LoggedInName { get; set; }
    }
}
