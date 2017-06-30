using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CodeFirstEntities
{
    [Serializable]
    public class GetCashRowsBySO
    {
        [Key]
        public int Id { get; set; }
        public string BillNo { get; set; }
    }
}
