using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class logindetail
    {
        [Key]
        public int loginid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
