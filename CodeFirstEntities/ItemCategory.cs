using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class ItemCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string itemCategoryStatus { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ItemCategoryCode { get; set; }
    }
}
