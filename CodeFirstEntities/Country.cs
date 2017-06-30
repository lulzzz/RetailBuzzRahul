using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public  class Country
    {
        [Key]
        public int countryid { get; set; }
        public string countryName { get; set; }
    }
}
