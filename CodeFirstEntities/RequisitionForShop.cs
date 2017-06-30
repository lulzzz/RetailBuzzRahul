using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
  public class RequisitionForShop
    {
      [Key]
      public int Id { get; set; }
      public string ItemCode { get; set; }
      public string ItemName { get; set; }
      public string Description { get; set; }
      public double? ShpQty { get; set; }
      public double? ReqQuantity { get; set; }
      public double? RejectQuantity { get; set; }
      public string LoggedInShopName { get; set; }
      public string RequisitionFrom { get; set; }
      //[Required]
      public string ToGodownName { get; set; }
      //[Required]
      public string ToShopName { get; set; }
      public string RequestToPo { get; set; }
     // public string RequisitionFromGodownName { get; set; }
      [Required]
      public string RequisitionForGodownPrepareBy { get; set; }
      public string RequisitionForGodownEmailIdEmployee { get; set; }
      public string RequisitionForGodownContactNoEmployee { get; set; }
      public DateTime? RequisitionForGodownPrepareTimeEmployee { get; set; }
      public string PrepareBy { get; set; }
      public string EmailIdEmployee { get; set; }
      public string ContactNoEmployee { get; set; }
      public string EmpName { get; set; }
      public string EmpDesignation { get; set; }
      public string EmpContactNo { get; set; }
      public string EmpEmail { get; set; }
      public double? GodownQty { get; set; }
      public double? Balance { get; set; }
      public double? OutwardQuantity { get; set; }
      public DateTime? RequsitionDate { get; set; }
      public DateTime? ModifiedOn { get; set; }
      public double? ShpQtyShopStock { get; set; }
      public double? RequiredQtyForGodown { get; set; }
      public string SubCategory { get; set; }
      public string codevaluerequisition { get; set; }
      public string ReqCode { get; set; }
      public string Category { get; set; }
      public string Status { get; set; }
    }
}
