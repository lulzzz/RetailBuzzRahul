using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CodeFirstEntities
{
    [Serializable]
    public class DesignationMaster
    {
        [Key]
        public int Id { get; set; }
        public string DesignationCode { get; set; }
        [Required]
        public string DesignationName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Status { get; set; }
    }
}
