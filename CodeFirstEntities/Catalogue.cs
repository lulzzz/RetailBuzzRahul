using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class Catalogue
    {
        [Key]
        public int Id { get; set; }
        public string BookNumber { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string SerialNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public string DesignCode { get; set; }
        public string Design { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string SupplierName { get; set; }
        public string Size { get; set; }
        public string TypeOfMaterial { get; set; }
        public string Unit { get; set; }
        public string NumberType { get; set; }
        public string CatalogueBarcode { get; set; }
        public string InwardBarcode { get; set; }
        public string CostPrice { get; set; }
        public string SellingPrice { get; set; }
        public string MRP { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
