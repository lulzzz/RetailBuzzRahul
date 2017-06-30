using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<Company> getAllCompanies();
        IEnumerable<Company> GetCompany(string Company1);
        IEnumerable<Company> getActiveCompany();
        Company getCompanyByName(string name);
        Company getCompanyByEmail(string email);
        Company getLastInsertedCompany();
        Company getById(int id);
        void CreateCompany(Company Company);
        void UpdateCompany(Company Company);
        IEnumerable<Company> getcombyname(string name);
        IEnumerable<Company> GetEmail(string mail);
        List<string> GetBank(string name);
        IEnumerable<Company> GetComponyList(string name);
        IEnumerable<Company> GetCompanyDataByUSerId(string ProcName, object[] id);
        List<CreateDynamicDb> CreateDynamicDatabase(string ProcName, object[] id);
        List<UpdateDynamicDb> UpdateDynamicDatabase(string ProcName, object[] id);
        List<BackUpDatabase> BackUpDatabase(string ProcName);
        Company GetCompanyDataByCompCode(string compCode);
        List<TruncateTable> TruncateTable(string ProcName);
        List<ValidationCompany> GetCompanyCounter(string ProcName);
    }
}
