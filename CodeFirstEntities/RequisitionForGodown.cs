using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
 public class RequisitionForGodown
    {
     [Key]
     public int Id { get; set; }
     public string ItemCode { get; set; }
     public string ItemName { get; set; }
     public string Description { get; set; }
     public string RequisitionFrom { get; set; }
     public double? QuantityInStock { get; set; }
     public double? RequiredQuantity { get; set; }
     public double? RejectQuantity { get; set; }
     public string ToGodownName { get; set; }
     public string RequestToPo { get; set; }
     public string LoggedInGodownName { get; set; }
     public string PrepareBy { get; set; }
     public string EmailIdEmployee { get; set; }
     public string ContactNoEmployee { get; set; }
     public DateTime? PrepareTimeEmployee { get; set; }
     public string RequisitionForGodownPrepareBy { get; set; }
     public string RequisitionForGodownEmailIdEmployee { get; set; }
     public string RequisitionForGodownContactNoEmployee { get; set; }
     public DateTime? RequisitionForGodownPrepareTimeEmployee { get; set; }
     public string EmpName { get; set; }
     public string EmpDesignation { get; set; }
     public string EmpContactNo { get; set; }
     public string EmpEmail { get; set; }
     public string DesignationEmployee { get; set; }
     public double? Balance { get; set; }
     public double? OutwardQuantity { get; set; }
     public DateTime? RequsitionDate { get; set; }
     public double? GodownQty { get; set; }
     public DateTime? ModifiedOn { get; set; }
     public string codevaluerequisition { get; set; }
     public string ReqCode { get; set; }
     public string Status { get; set; }
     public string ToShopName { get; set; }
     public double? ShopQty { get; set; }
     public string SubCategory { get; set; }
     public string Category { get; set; }
    }
}
