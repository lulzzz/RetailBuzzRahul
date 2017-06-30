using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class BankName
    {
        [Key]
        public int bankId { get; set; }
        public string bankName { get; set; }
    }
}
