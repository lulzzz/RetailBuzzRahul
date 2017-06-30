using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class Item
    {
        [Key]
        public int itemId { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string itemCategory { get; set; }
        public string itemSubCategory { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime? modifedOn { get; set; }
        //[Required]
        public string costprice { get; set; }
        //[Required]
        public string typeOfMaterial { get; set; }
        [Required]
        public string colorCode { get; set; }
        public string designCode { get; set; }
        //[Required]
        public string sellingprice { get; set; }
        //[Required]
        public string mrp { get; set; }
        public string brandName { get; set; }
        public string size { get; set; }
        public string designName { get; set; }
        public string unit { get; set; }
        public string NumberType { get; set; }
        public string itemtype { get; set; }
        public string Barcode { get; set; }
        public string UniversalBarcode { get; set; }
    }
}
