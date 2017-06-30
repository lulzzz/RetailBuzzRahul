using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IEmployeesCompanyService
    {
        void Create(EmployeesCompany CompanyCode);
        IEnumerable<EmployeesCompany> GetAllEmployeeCompaniesByEmpCode(string empCode);
        IEnumerable<EmployeesCompany> GetAllEmployeeCompanies();
    }
}
