using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class UnitConversion
    {
        [Key]
        public int Id { get; set; }
        public string FromUnit { get; set; }
        public string Value { get; set; }
        public string ToUnit { get; set; }
    }
}
