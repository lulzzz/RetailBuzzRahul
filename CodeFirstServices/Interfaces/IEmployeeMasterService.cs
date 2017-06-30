using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public  interface IEmployeeMasterService
    {
        IEnumerable<EmployeeMaster> getAllemployees();
        IEnumerable<EmployeeMaster> GetEmployee(string employee);
        IEnumerable<EmployeeMaster> GetEmployeeByStatus();
        IEnumerable<EmployeeMaster> getActiveEmployee();
        EmployeeMaster getEmpByName(string name);
        EmployeeMaster getEmpByEmail(string Email);
        EmployeeMaster getLastInsertedEmp();
        EmployeeMaster getById(int id);
        void CreateEmployee(EmployeeMaster employee);
        void UpdateEmployee(EmployeeMaster employee);
        void SaveEmployee();
        IEnumerable<EmployeeMaster> getEmpDetailsByEmail(string email);
        IEnumerable<EmployeeMaster> getEmpDetailsByDesignation(string designation);
        EmployeeMaster getEmpById(int id);
        IEnumerable<EmployeeMaster> getEmpDetailsById(int id);
        IEnumerable<EmployeeMaster> getAllEmployess(int id);
        EmployeeMaster getEmpDetailsByBankName(string bankname);
        string getBankIfscNoByName(string bankname);
        IEnumerable<EmployeeMaster> GetEmail(string mail);
        IEnumerable<EmployeeMaster> GetEmployeeByDepartment(string employee);
        IEnumerable<EmployeeMaster> GetEmpByNameAndSalesDepartment(string name);
        IEnumerable<EmployeeMaster> GetEmpByDesignation();
        EmployeeMaster GetEmpByCode(string code);
        IEnumerable<EmployeeMaster> GetEmpBySalesDesignation(string name);
        EmployeeMaster GetEmpRowBySalesDesignation(string name);
        IEnumerable<EmployeeMaster> GetShopEmp(string name);
        IEnumerable<EmployeeMaster> GetEmployeesBySalesDepartment();
        EmployeeMaster GetEmpDetailsOfStoreKeeper();
        List<string> GetEmployeesByTarget(string name);
        IEnumerable<EmployeeMaster> GetEmployeeListBySalesAndTarget();
        IEnumerable<EmployeeMaster> GetEmployeeListByDepartment(string DepartmentName);
    }
}
