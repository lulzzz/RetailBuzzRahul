using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class GodownAddress
    {
        [Key]
        public int AddressId { get; set; }
        public string GdCode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string AreaName { get; set; }
        public string Location { get; set; }
    }
}
