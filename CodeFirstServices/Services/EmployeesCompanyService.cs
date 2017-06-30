using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class EmployeesCompanyService : IEmployeesCompanyService
    {
        private readonly IEmployeesCompanyRepository _EmployeesCompanyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesCompanyService(IEmployeesCompanyRepository EmployeesCompanyRepository, IUnitOfWork unitOfWork, ICreateDynamicDbRepository CreateDynamicDbRepository)
        {
            this._EmployeesCompanyRepository = EmployeesCompanyRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(EmployeesCompany CompanyCode)
        {
            _EmployeesCompanyRepository.Add(CompanyCode);
            _unitOfWork.Commit();
        }

        public IEnumerable<EmployeesCompany> GetAllEmployeeCompaniesByEmpCode(string empCode)
        {
            var list = _EmployeesCompanyRepository.GetMany(m => m.UserCode == empCode).OrderByDescending(m => m.CompanyCode);
            return list;
        }

        public IEnumerable<EmployeesCompany> GetAllEmployeeCompanies()
        {
            var list = _EmployeesCompanyRepository.GetAll();
            return list;
        }
    }
}
