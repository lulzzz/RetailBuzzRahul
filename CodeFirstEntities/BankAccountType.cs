using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class BankAccountType
    {
        [Key]
        public int accountTypeId { get; set; }
        public string accountType { get; set; }
    }
}
