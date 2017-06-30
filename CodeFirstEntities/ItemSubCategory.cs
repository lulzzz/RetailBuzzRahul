using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class ItemSubCategory
    {
        [Key]
        public int subCategoryId { get; set; }
        [Required]
        public string subCategoryName { get; set; }
        [Required]
        public string categoryName { get; set; }
        public string status { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ItemSubCategoryCode { get; set; }
    }
}
