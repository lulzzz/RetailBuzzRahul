using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MvcRetailApp.ModelBinder
{
    public class EmployeeMasterCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int empId = Convert.ToInt32(request.Form.Get("EmployeeDetails.EmpId"));
            string Fname = request.Form.Get("EmployeeDetails.FirstName");
            string Lname = request.Form.Get("EmployeeDetails.LastName");
            string Add = request.Form.Get("EmployeeDetails.Address");
            DateTime DOB = Convert.ToDateTime(request.Form.Get("EmployeeDetails.DateOfBirth"));
            string contactNo = (request.Form.Get("EmployeeDetails.MobileNo"));
            string residenceno = (request.Form.Get("EmployeeDetails.ResidenceNo"));
            string email = request.Form.Get("EmployeeDetails.Email");
            string designation = request.Form.Get("EmployeeDetails.Designation");
            DateTime DOJ = Convert.ToDateTime(request.Form.Get("EmployeeDetails.DateOfJoining"));
            // DateTime DOJ =(request.Form.Get("txtCalendarFrDOB"));
            string Industry = request.Form.Get("EmployeeDetails.Industry");
            string state = request.Form.Get("EmployeeDetails.State");
            string city = request.Form.Get("EmployeeDetails.City");
            int yr = Convert.ToInt32(request.Form.Get("EmployeeDetails.totalExpYear"));
            int month = Convert.ToInt32(request.Form.Get("EmployeeDetails.totalExpmonth"));
            int sal = Convert.ToInt32(request.Form.Get("EmployeeDetails.Salary"));
            string PanNo = request.Form.Get("EmployeeDetails.PancardNo");
            string BG = request.Form.Get("EmployeeDetails.BloodGroup");
            string Esic = request.Form.Get("EmployeeDetails.ESIC");
            string pfno = request.Form.Get("EmployeeDetails.PFNO");          
            string bankname = request.Form.Get("EmployeeDetails.BankName");
            string banklocatiom = request.Form.Get("EmployeeDetails.BankAddress");
            string bankAccount = request.Form.Get("EmployeeDetails.AccountType");
            string bankifsc = request.Form.Get("EmployeeDetails.BankIFSCNO");
            string bankaccountno = request.Form.Get("EmployeeDetails.Account_No");
            string branch = request.Form.Get("EmployeeDetails.branch");
            string empcode = request.Form.Get("EmployeeDetails.EmployeeCode");       
            string status = "Active";
            DateTime modifiedOn = DateTime.Now;
            return new EmployeeMaster
            {
                EmpId = empId,
                Name = Fname,
               // LastName = Lname,
                Address = Add,
                DateOfBirth = DOB,
                MobileNo=contactNo,
                ResidenceNo=residenceno,
                Email = email,
                Designation = designation,
                DateOfJoining = DOJ,
                Industry = Industry,
                State = state,
                City = city,
                totalExpYear = yr,
                totalExpmonth = month,
                Salary = sal,
                PancardNo = PanNo,
                BloodGroup = BG,
                ESIC=Esic,
                PFNO=pfno,
                BankName=bankname,
                BankAddress=banklocatiom,
                AccountType =bankAccount,
                BankIFSCNo=bankifsc,
                Account_No=bankaccountno,
                EmployeeCode=empcode,
                branch=branch,
                Status = status,
                ModifiedOn = modifiedOn
            };
        }
    }
}