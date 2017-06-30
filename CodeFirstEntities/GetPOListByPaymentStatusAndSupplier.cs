using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CodeFirstEntities
{
    [Serializable]
    public class GetPOListByPaymentStatusAndSupplier
    {
        [Key]
        public int Id { get; set; }
        public string PONo { get; set; }
    }
}

