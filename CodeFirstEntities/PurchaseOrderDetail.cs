using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class PurchaseOrderDetail
    {
        [Key]
        public int PoId { get; set; }
        public string PoNo { get; set; }
        public DateTime PoDate { get; set; }
        public DateTime DelDate { get; set; }
        [Required]
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContactNo { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierState { get; set; }
        [Required]
        public string ModeOfTransport { get; set; }
        [Required]
        public string TransportName { get; set; }
        public string TransportContactNo { get; set; }
        public string DelAt { get; set; }
        public DateTime modifiedOn { get; set; }
        public string status { get; set; }

        public string GodownCode { get; set; }
        public string GodownName { get; set; }
        public string GodownArea { get; set; }
        public string GodownEmail { get; set; }
        public string GodownContactNo { get; set; }
        public string GodownContactPerson { get; set; }

        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string ShopEmail { get; set; }
        public string ShopContactPerson { get; set; }
        public string ShopContactNo { get; set; }

        public double? TotalAmount { get; set; }
        public double? GrandTotal { get; set; }
        public string ReceivingStatus { get; set; }
        public string FormType { get; set; }
        public string ClientType { get; set; }
        public double? PackAndForwd { get; set; }
        public string Narration { get; set; }
        public string SalesPerson { get; set; }
    }
}
