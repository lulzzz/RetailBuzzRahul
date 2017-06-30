using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstEntities
{
    [Serializable]
    public class CommissionProduct
    {
        [Key]
        public int Id { get; set; }
        public string CommissionCode { get; set; }
        public string EmployeeName { get; set; }
        public string UnitName { get; set; }
        public string UnitAmount { get; set; }
        public double UnitPercent { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
