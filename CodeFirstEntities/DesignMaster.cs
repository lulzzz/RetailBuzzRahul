using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class DesignMaster
    {
        [Key]
        public int DesignId { get; set; }
        public string DesignCode { get; set; }
        public string CategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        [Required]
        public string DesignName { get; set; }
        //[Required]
        public string DesignDescription { get; set; }
        public string DesignPhoto { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}