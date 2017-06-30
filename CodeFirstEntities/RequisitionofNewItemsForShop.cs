using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CodeFirstEntities
{
  public  class RequisitionofNewItemsForShop
    {
      [Key]
      public int Id { get; set; }
      public string ItemName { get; set; }
      public string ItemCode { get; set; }
      public int Quantity { get; set; }
     public string BrandName { get; set; }
      public string Description { get; set; }
      public string ShopOrGodownName { get; set; }
      public DateTime ModifiedOn { get; set; }
    }
}
