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
    public class GodownMasterService:IGodownService
    {
        private readonly IGodownRepository _godownmasterRepository;
        private readonly IUnitOfWork _unitofwork;
        private readonly IGetUsersEmailRepository _GetUserEmailRepository;

        public GodownMasterService(IGodownRepository godownmasterRepository, IUnitOfWork unitofwork, IGetUsersEmailRepository GetUserEmailRepository)
        {
            this._godownmasterRepository = godownmasterRepository;
            this._unitofwork = unitofwork;
            this._GetUserEmailRepository = GetUserEmailRepository;
        }

        public GodownMaster getGodownLast()
        {
            var godown = _godownmasterRepository.GetAll().LastOrDefault();
            return godown;
        }

        public void CreateGodown(GodownMaster godownmaster)
        {
            _godownmasterRepository.Add(godownmaster);
            _unitofwork.Commit();
        }
        public GodownMaster GetGodownDetailsById(int id)
        {
            var details = _godownmasterRepository.Get(d => d.GodownId == id);
            return details;
        }
       
        public IEnumerable<GodownMaster> GetEmailList(string email)
        {
            var list = _godownmasterRepository.GetMany(l => l.GodownEmail == email);
            return list;
        }

        public IEnumerable<GodownMaster> GetGodownNames()
        {
            var list = _godownmasterRepository.GetMany(m => m.Status == "Active");
            return list;
        }

        public IEnumerable<GodownMaster> GetGodownNamesExcludingOne(string name)
        {
            var list = _godownmasterRepository.GetMany(m => m.Status == "Active"  && m.GodownName!=name);
            return list;
        }
        public GodownMaster GetGodownByCode(string code)
        {
            var data = _godownmasterRepository.Get(gd => gd.GdCode == code);
            return data;
        }

        public void UpdateGodown(GodownMaster godown)
        {
            _godownmasterRepository.Update(godown);
            _unitofwork.Commit();
        }

        public void DeleteGodown(GodownMaster godown)
        {
            _godownmasterRepository.Delete(godown);
            _unitofwork.Commit();
        }

        public IEnumerable<GodownMaster> GetGodownList(string name)
        {
            var list = _godownmasterRepository.GetMany(l1 => l1.GodownName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.GodownName);
            return list;
        }

        public IEnumerable<GetUsersEmail> GetUserEmails(string procname)
        {
            var data = _GetUserEmailRepository.ExecuteCustomStoredProc(procname);
            return data;
        }

        public GodownMaster GetGodownDetailsByName(string name)
        {
            var details = _godownmasterRepository.Get(s => s.GodownName == name);
            return details;
        }

        public GodownMaster CheckShortCode(string Code)
        {
            var details = _godownmasterRepository.Get(gd => gd.ShortCode == Code);
            return details;
        }

        public IEnumerable<GodownMaster> GetAll()
        {
            var list = _godownmasterRepository.GetMany(l => l.Status == "Active");
            return list;
        }
    }
}
