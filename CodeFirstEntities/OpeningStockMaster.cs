using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace CodeFirstEntities
{
     [Serializable]
  public class OpeningStockMaster
    {
      [Key]
      public int OpeningStockId { get; set; }
      public string OpeningStockCode { get; set; }
      public string Category { get; set; }
      public string SubCategory { get; set; }
      public string ItemName { get; set; }
      public string Description { get; set; }
      public double? ItemQuantity { get; set; }
      public double? CostPrice { get; set; }
      public double? SellingPrice { get; set; }
      public double? MRP { get; set; }
      public string Color { get; set; }
      public string Size { get; set; }
      public string Unit { get; set; }
      public string NumberType { get; set; }
      public string Status { get; set; }
      public DateTime? ModifiedOn { get; set; }
      public string Barcode { get; set; }
      public string itemCode { get; set; }
      public double? TotalQuantity { get; set; }
      public string Material { get; set; }
      public string DesignCode { get; set; }
      public string DesignName { get; set; }
      public string Brand { get; set; }
   }
}
