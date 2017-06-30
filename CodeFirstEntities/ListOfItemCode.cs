using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CodeFirstEntities
{
    [Serializable]
    public class ListOfItemCode
    {
        [Key]
        public int id { get; set; }
        public string itemCode { get; set; }
    }
}
