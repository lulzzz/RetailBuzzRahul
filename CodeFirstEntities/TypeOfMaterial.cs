using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class TypeOfMaterial
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        public string MaterialName { get; set; }
        [Required]
        public string MaterialShortName { get; set; }
        [Required]
        public string MaterialDescription { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Status { get; set; }
        public string MaterialCode { get; set; }
    }
}

