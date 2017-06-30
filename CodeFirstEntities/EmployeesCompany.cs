using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class EmployeesCompany
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string CompanyCode { get; set; }
    }
}
