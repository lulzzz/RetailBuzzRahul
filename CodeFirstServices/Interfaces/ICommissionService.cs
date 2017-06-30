using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface ICommissionService
    {
        CommissionMaster GetLast();
        void Create(CommissionMaster Comm);
        CommissionMaster GetDetailsById(int Id);
        CommissionMaster CheckDate(DateTime FromDate, DateTime ToDate,string Name);
        CommissionMaster GetCommByEmployeeName(string Name,DateTime FromDate, DateTime ToDate);
        IEnumerable<CommissionMaster> GetAllEmployees();
        CommissionMaster GetDetailsByEmployeeName(string Name);
        void Update(CommissionMaster comm);
        IEnumerable<CommissionMaster> GetEmployeesByDate(DateTime FromDate, DateTime ToDate);
        CommissionMaster GetDetailsByCommCode(string Code);
        CommissionMaster GetRowByDateAndName(DateTime FromDate, DateTime ToDate, string Name);
        IEnumerable<CommissionMaster> CommissionGivenEmployees(DateTime FromDate, DateTime ToDate);
        IEnumerable<CommissionMaster> GetAll();
    }
}
