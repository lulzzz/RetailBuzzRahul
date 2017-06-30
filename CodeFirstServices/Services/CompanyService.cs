using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateDynamicDbRepository _CreateDynamicDbRepository;
        private readonly IUpdateDynamicDbRepository _UpdateDynamicDbRepository;
        private readonly IBackUpDatabaseRepository _BackUpDatabaseRepository;
        private readonly ITruncateTableRepository _TruncateTableRepository;
        private readonly IValidationCompanyRepository _ValidationCompanyRepository;
        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, ICreateDynamicDbRepository CreateDynamicDbRepository, IUpdateDynamicDbRepository UpdateDynamicDbRepository, IBackUpDatabaseRepository BackUpDatabaseRepository, ITruncateTableRepository TruncateTableRepository, IValidationCompanyRepository ValidationCompanyRepository)
        {
            this._companyRepository = companyRepository;
            this._unitOfWork = unitOfWork;
            this._CreateDynamicDbRepository = CreateDynamicDbRepository;
            this._UpdateDynamicDbRepository = UpdateDynamicDbRepository;
            this._BackUpDatabaseRepository = BackUpDatabaseRepository;
            this._TruncateTableRepository = TruncateTableRepository;
            this._ValidationCompanyRepository = ValidationCompanyRepository;
        }

        public Company getById(int id)
        {
            var companydata = _companyRepository.GetById(id);
            return companydata;
        }

        public List<string> GetBank(string name)
        {
            var bnk = _companyRepository.GetMany(bank => bank.companyName.ToLower().StartsWith(name.ToString().ToLower()) && bank.isEnabled == "Active").OrderBy(bank => bank.companyName);
            var bnk1 = bnk.Select(ba => ba.companyName).ToList();
            return bnk1;
        }

        public Company getByName(string companyname)
        {
            var company = _companyRepository.Get(cmp => cmp.companyName == companyname);
            return company;
        }

        public Company getCompanyByName(string name)
        {
            var companyName = _companyRepository.Get(cmp => cmp.companyName == name);
            return companyName;
        }

        public Company getCompanyByEmail(string email)
        {
            var companyName = _companyRepository.Get(cmp => cmp.eMail == email);
            return companyName;
        }

        public IEnumerable<Company> getActiveCompany()
        {
            Func<Company, bool> filter = (Company p) => p.isEnabled == "Active";
            IEnumerable<Company> company = _companyRepository.GetMany(filter);
            return company;
        }

        public Company getLastInsertedCompany()
        {
            var company = _companyRepository.GetAll().LastOrDefault();
            return company;
        }

        public IEnumerable<Company> GetCompany(string Company1)
        {
            var cmp1 = _companyRepository.GetMany(cmp => cmp.companyName.ToLower().StartsWith(Company1.ToString().ToLower()) && cmp.isEnabled == "Active");
            return cmp1;
        }

        public IEnumerable<Company> getcombyname(string email)
        {
            var companyName = _companyRepository.GetMany(cmp => cmp.companyName == email);
            return companyName;
        }

        public IEnumerable<Company> GetEmail(string mail)
        {
            var list = _companyRepository.GetMany(cmp => cmp.eMail == mail);
            return list;
        }

        public IEnumerable<Company> getAllCompanies()
        {
            var Company = _companyRepository.GetAll();
            return Company;
        }

        public void CreateCompany(Company Company)
        {
            _companyRepository.Add(Company);
            _unitOfWork.Commit();
        }

        public void UpdateCompany(Company Company)
        {
            _companyRepository.Update(Company);
            _unitOfWork.Commit();
        }

        public void DeleteCompany(int id)
        {
            var company = _companyRepository.GetById(id);
            _companyRepository.Delete(company);
            _unitOfWork.Commit();
        }

        public void SaveCompany()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Company> GetComponyList(string name)
        {
            var list = _companyRepository.GetMany(l => l.companyName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.companyName);
            return list;
        }

        public IEnumerable<Company> GetCompanyDataByUSerId(string ProcName, object[] id)
        {
            var companyList = _companyRepository.ExecuteCustomStoredProcByParam(ProcName, id);
            return companyList;
        }

        public List<CreateDynamicDb> CreateDynamicDatabase(string ProcName, object[] id)
        {
            var list = _CreateDynamicDbRepository.ExecuteCustomStoredProcByParam(ProcName, id);
            return list;
        }

        public List<UpdateDynamicDb> UpdateDynamicDatabase(string ProcName, object[] id)
        {
            var list = _UpdateDynamicDbRepository.ExecuteCustomStoredProcByParam(ProcName, id);
            return list;
        }

        public Company GetCompanyDataByCompCode(string compCode)
        {
            var data = _companyRepository.Get(m => m.CompanyCode == compCode);
            return data;
        }

        public List<BackUpDatabase> BackUpDatabase(string ProcName)
        {
            var list = _BackUpDatabaseRepository.ExecuteCustomStoredProc(ProcName);
            return list;
        }

        public List<TruncateTable> TruncateTable(string ProcName)
        {
            var list = _TruncateTableRepository.ExecuteCustomStoredProc(ProcName);
            return list;
        }

        public List<ValidationCompany> GetCompanyCounter(string ProcName)
        {
            var list = _ValidationCompanyRepository.ExecuteCustomStoredProc(ProcName);
            return list;
        }
    }
}
