using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
  public  class EmployeeMasterService :IEmployeeMasterService
    {
      private readonly IEmployeeMasterRepository _EmployeeMasterRepository;
      private readonly IUnitOfWork _unitOfWork;


      public EmployeeMasterService(IEmployeeMasterRepository EmployeeMasterRepository, IUnitOfWork unityOfWork)
      {
          this._EmployeeMasterRepository = EmployeeMasterRepository;
          this._unitOfWork = unityOfWork; 
      }

      public IEnumerable<EmployeeMaster> getAllemployees()
      {
          var emp = _EmployeeMasterRepository.GetMany(m => m.Status == "Active");
          return emp;
      }

      public IEnumerable<EmployeeMaster> getEmpDetailsByDesignation(string designation)
      {
          var empdetails = _EmployeeMasterRepository.GetMany(emp => emp.Designation == designation && emp.Status=="Active");
          return empdetails;

      }
       public string getBankIfscNoByName(string bankname)
       {
           var ifsc = _EmployeeMasterRepository.Get(bn => bn.BankName == bankname);
           return ifsc.BankIFSCNo;

       }

      public IEnumerable<EmployeeMaster> getEmpDetailsById(int id)
      {
          var empdetails = _EmployeeMasterRepository.GetMany(emp => emp.EmpId == id);
          return empdetails;

      }
      public EmployeeMaster getEmpDetailsByBankName(string bankname)
      {
          var empdetails = _EmployeeMasterRepository.Get(emp => emp.BankName==bankname);
          return empdetails;

      }
      public IEnumerable<EmployeeMaster> getAllEmployess(int id)
      {
          var empdetails = _EmployeeMasterRepository.GetMany(emp => emp.EmpId == id && emp.Status=="Active");
          return empdetails;
      }

      public EmployeeMaster getEmpById(int id)
      {
          var empdetails = _EmployeeMasterRepository.Get(emp => emp.EmpId == id);
          return empdetails;
      }
      public IEnumerable<EmployeeMaster> GetEmployee(string employee)
      {
          var emp1 = _EmployeeMasterRepository.GetMany(emp => emp.Name.ToLower().StartsWith(employee.ToString().ToLower()) && emp.Status == "Active");
          return emp1;
      }

      public IEnumerable<EmployeeMaster> GetEmployeeByStatus()
      {
          var emp = _EmployeeMasterRepository.GetMany(e=>e.Status=="Active");
          return emp;
      }

      public IEnumerable<EmployeeMaster> getActiveEmployee()
      {
          Func<EmployeeMaster, bool> filter = (EmployeeMaster p) => p.Status == "Active";
          IEnumerable<EmployeeMaster> emp = _EmployeeMasterRepository.GetMany(filter);
          return emp;
      }

      public EmployeeMaster getEmpByName(string name)
      {
          var EmpName = _EmployeeMasterRepository.Get(emp => emp.Name == name);
          return EmpName;
      }
      public EmployeeMaster getEmpByEmail(string Email)
      {
          var Emp = _EmployeeMasterRepository.Get(emp => emp.Email == Email);
          return Emp;
      }

     
      public IEnumerable<EmployeeMaster> getEmpDetailsByEmail(string email)
      {
          var empdetails = _EmployeeMasterRepository.GetMany(emp => emp.Email == email);
          return empdetails;

      }

      public EmployeeMaster getLastInsertedEmp()
      {
          var emp = _EmployeeMasterRepository.GetAll().LastOrDefault();
          return emp;
      }
     

      public EmployeeMaster getById(int id)
      {
          var emp = _EmployeeMasterRepository.GetById(id);
          return emp;
      }

      public void CreateEmployee(EmployeeMaster employee)
      {
          _EmployeeMasterRepository.Add(employee);
          _unitOfWork.Commit();
      }

      public void UpdateEmployee(EmployeeMaster employee)
      {  _EmployeeMasterRepository.Update(employee);
         _unitOfWork.Commit();
          
      }

      public IEnumerable<EmployeeMaster> GetEmail(string mail)
      {
          var EmployeeList = _EmployeeMasterRepository.GetMany(emp=>emp.Email==mail);
          return EmployeeList;
      }

      public IEnumerable<EmployeeMaster> GetEmployeeByDepartment(string employee)
      {
          var list = _EmployeeMasterRepository.GetMany(emp => emp.Name.ToLower().StartsWith(employee.ToString().ToLower()) && emp.Status == "Active" && emp.department == "Godown");
          return list;
      }

      public void SaveEmployee()
      {
          _unitOfWork.Commit();
      }

      public IEnumerable<EmployeeMaster> GetEmpByNameAndSalesDepartment(string name)
      {
          var list = _EmployeeMasterRepository.GetMany(emp => emp.Name.ToLower().StartsWith(name.ToString().ToLower()) && emp.Status == "Active" && emp.department == "Sales");
          return list;
      }

      public IEnumerable<EmployeeMaster> GetEmpByDesignation()
      {
          var emp = _EmployeeMasterRepository.GetMany(e => e.Designation == "Accountant" || e.Designation == "Cashier" && e.Status == "Active");
          return emp;
      }

      public EmployeeMaster GetEmpDetailsOfStoreKeeper()
      {
          var emp = _EmployeeMasterRepository.Get(e => e.Designation == "StoreKeeper" );
          return emp;
      }

      public EmployeeMaster GetEmpByCode(string code)
      {
          var empdetails = _EmployeeMasterRepository.Get(e => e.EmployeeCode == code && e.Status == "Active");
          return empdetails;
      }

      public IEnumerable<EmployeeMaster> GetEmpBySalesDesignation(string name)
      {
          var empployee = _EmployeeMasterRepository.GetMany(emp => emp.Name.ToLower().StartsWith(name.ToString().ToLower()) && emp.Status == "Active" && (emp.Designation == "SalesOfficer" || emp.Designation == "SalesMan"));
          return empployee;
      }

      public EmployeeMaster GetEmpRowBySalesDesignation(string name)
      {
          var details = _EmployeeMasterRepository.Get(m => m.Name == name);
          return details;
      }

      public IEnumerable<EmployeeMaster> GetShopEmp(string name)
      {
          var empployee = _EmployeeMasterRepository.GetMany(emp => emp.Name.ToLower().StartsWith(name.ToString().ToLower()) && emp.Status == "Active" && (emp.Designation == "StoreKeeper" || emp.Designation == "StoreAssistant"));
          return empployee;
      }

      public IEnumerable<EmployeeMaster> GetEmployeesBySalesDepartment()
      {
          var details = _EmployeeMasterRepository.GetMany(m => m.department == "Sales");
          return details;
      }

      public List<string> GetEmployeesByTarget(string name)
      {
          var details = _EmployeeMasterRepository.GetMany(e => e.Name == name);
          List<string> EmpName = new List<string>();
          foreach (var data in details)
          {
              EmpName.Add(data.Name);
          }
          return EmpName;
      }


      public IEnumerable<EmployeeMaster> GetEmployeeListBySalesAndTarget()
      {
          var details = _EmployeeMasterRepository.GetMany(e => e.department == "Sales" && e.CommissionType == "Target");
          return details;
      }


      public IEnumerable<EmployeeMaster> GetEmployeeListByDepartment(string DepartmentName)
      {
          var details = _EmployeeMasterRepository.GetMany(e => e.department == DepartmentName && e.Status == "Active");
          return details;
      }
    }
}
