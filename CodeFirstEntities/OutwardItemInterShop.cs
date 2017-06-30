using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
  public class OutwardItemInterShop
    {
      [Key]
      public int ItemId { get; set; }
      public string OutwardCode { get; set; }
      public string Barcode { get; set; }
      public string ItemCode { get; set; }
      public string ItemName { get; set; }
      public string Description { get; set; }
      public string TypeOfMaterial { get; set; }
      public string ColorCode { get; set; }
      public double? SellingPrice { get; set; }
      public double? MRP { get; set; }
      public string DesignCode { get; set; }
      public string DesignName { get; set; }
      public string FromShopCode { get; set; }
      public string ToShopCode { get; set; }
      public string Unit { get; set; }
      public string NumberType { get; set; }
      public double? QuantityTransfer { get; set; }
      public string Size { get; set; }
      public string Brand { get; set; }
      public double? Balance { get; set; }
      public string Status { get; set; }
      public DateTime ModifiedOn { get; set; }

    }
}
