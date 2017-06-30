using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class BarcodeTempDetail
    {
        [Key]
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string typeOfMaterial { get; set; }
        public string colorCode { get; set; }
        public string designCode { get; set; }
        public string itemCategory { get; set; }
        public string itemSubCategory { get; set; }
    }
}
