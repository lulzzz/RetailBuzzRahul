using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class BarcodeNumber
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public Byte[] BarcodeImage { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
