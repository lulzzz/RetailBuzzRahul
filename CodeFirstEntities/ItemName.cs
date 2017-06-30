using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class ItemName
    {
        [Key]
        public int Id { get; set; }
        public string ItemCategory { get; set; }
        public string ItemSubcategory { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ItemType { get; set; }
        public string DeleteStatus { get; set; }
        public string Status { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
