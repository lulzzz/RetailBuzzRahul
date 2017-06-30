using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class InventoryTax
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Tax { get; set; }
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
    }
}
