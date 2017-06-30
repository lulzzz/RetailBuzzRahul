using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
     [Serializable]
    public class TypeOfSupplier
    {
        [Key]
        public int TypeOfSupplierId { get; set; }
        public string type_supplier { get; set; }
    }
}
