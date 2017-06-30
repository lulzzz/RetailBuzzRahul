using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class GetItemsByQuotOrOrderNo
    {
        [Key]
        public int Id { get; set; }
        public string ItemCode { get; set; }
    }
}
