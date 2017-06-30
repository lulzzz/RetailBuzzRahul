using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
     [Serializable]
  public class State
    {
        [Key]
        public int Stateid { get; set; }
        public string StateName { get; set; }
        public int Countryid { get; set; }
        public string CountryName { get; set; }        
    }
}
