using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
     [Serializable]
    public class YearExperience
    {
        [Key]
        public int expId { get; set; }
        public int noOfYear { get; set; }
    }
}
