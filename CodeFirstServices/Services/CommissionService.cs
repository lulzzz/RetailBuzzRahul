using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly ICommissionRepository _CommissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommissionService(ICommissionRepository CommissionRepository, IUnitOfWork unitOfWork)
        {
            this._CommissionRepository = CommissionRepository;
            this._unitOfWork = unitOfWork;
        }

        public CommissionMaster GetLast()
        {
            var details = _CommissionRepository.GetAll().LastOrDefault();
            return details;
        }

        public void Create(CommissionMaster Comm)
        {
            _CommissionRepository.Add(Comm);
            _unitOfWork.Commit();
        }

        public CommissionMaster GetDetailsById(int Id)
        {
            var details = _CommissionRepository.Get(c => c.CommissionId == Id);
            return details;
        }

        public CommissionMaster CheckDate(DateTime FromDate, DateTime ToDate, string Name)
        {
            var details = _CommissionRepository.Get(m => ((Convert.ToDateTime(m.FromDate) <= (FromDate.Date) && Convert.ToDateTime(m.ToDate) >= (FromDate.Date))
                || (Convert.ToDateTime(m.FromDate) <= (ToDate.Date) && Convert.ToDateTime(m.ToDate) >= (ToDate.Date))) && m.EmployeeName == Name);
            return details;
        }

        public CommissionMaster GetCommByEmployeeName(string Name, DateTime FromDate, DateTime ToDate)
        {
            var details = _CommissionRepository.Get(c => c.EmployeeName == Name && ((Convert.ToDateTime(c.FromDate) <= FromDate.Date && Convert.ToDateTime(c.ToDate) >= (FromDate.Date)) ||
                Convert.ToDateTime(c.FromDate) >= (FromDate.Date)));
            return details;
        }

        public IEnumerable<CommissionMaster> GetAllEmployees()
        {
            var details = _CommissionRepository.GetMany(c => c.Status == "Active");
            return details;
        }

        public CommissionMaster GetDetailsByEmployeeName(string Name)
        {
            var details = _CommissionRepository.Get(c => c.EmployeeName == Name);
            return details;
        }

        public void Update(CommissionMaster comm)
        {
            _CommissionRepository.Update(comm);
            _unitOfWork.Commit();
        }

        public IEnumerable<CommissionMaster> GetEmployeesByDate(DateTime FromDate, DateTime ToDate)
        {
            var details = _CommissionRepository.GetMany(m => ((Convert.ToDateTime(m.FromDate) <= (FromDate.Date) && Convert.ToDateTime(m.ToDate) >= (FromDate.Date))
                || (Convert.ToDateTime(m.FromDate) <= (ToDate.Date) && Convert.ToDateTime(m.ToDate) >= (ToDate.Date))) && m.Amount == null && m.Status == "Active");
            return details;
        }

        public CommissionMaster GetDetailsByCommCode(string Code)
        {
            var details = _CommissionRepository.Get(c => c.CommissionCode == Code);
            return details;
        }

        public CommissionMaster GetRowByDateAndName(DateTime FromDate, DateTime ToDate, string Name)
        {
            var details = _CommissionRepository.Get(m => Convert.ToDateTime(m.FromDate) == FromDate.Date && Convert.ToDateTime(m.ToDate) == ToDate.Date && m.EmployeeName == Name && m.Status == "Active");
            return details;
        }

        public IEnumerable<CommissionMaster> CommissionGivenEmployees(DateTime FromDate, DateTime ToDate)
        {
            var details = _CommissionRepository.GetMany(m => (Convert.ToDateTime(m.FromDate) >= (FromDate.Date) && Convert.ToDateTime(m.ToDate) <= (ToDate.Date)) && m.Amount != null && m.Status == "Active");
            return details;
        }

        public IEnumerable<CommissionMaster> GetAll()
        {
            var list = _CommissionRepository.GetAll();
            return list;
        }
    }
}
