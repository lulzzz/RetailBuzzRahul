using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class DesignationMasterService : IDesignationMasterService
    {
        private readonly IDesignationMasterRepository _DesignationMasterRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public DesignationMasterService(IDesignationMasterRepository DesignationMasterRepository, IUnitOfWork unitofwork)
        {
            this._DesignationMasterRepository = DesignationMasterRepository;
            this._UnitOfWork = unitofwork;
        }

        public void Create(DesignationMaster designation)
        {
            _DesignationMasterRepository.Add(designation);
            _UnitOfWork.Commit();
        }

        public void Update(DesignationMaster designation)
        {
            _DesignationMasterRepository.Update(designation);
            _UnitOfWork.Commit();
        }

        public IEnumerable<DesignationMaster> getAllDesignation()
        {
            var designationlist = _DesignationMasterRepository.GetAll();
            return designationlist;
        }

        public DesignationMaster getLastDesignation()
        {
            var lastrow = _DesignationMasterRepository.GetAll().LastOrDefault();
            return lastrow;
        }

        public IEnumerable<DesignationMaster> GetDesignationList(string name)
        {
            var list = _DesignationMasterRepository.GetMany(l1 => l1.DesignationName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.DesignationName);
            return list;
        }

        public IEnumerable<DesignationMaster> GetDesignationName(string name)
        {
            var namelist = _DesignationMasterRepository.GetMany(n => n.DesignationName == name);
            return namelist;
        }

        public DesignationMaster getById(int id)
        {
            var data = _DesignationMasterRepository.Get(it => it.Id == id);
            return data;
        }

        public IEnumerable<DesignationMaster> GetDesignations()
        {
            var list = _DesignationMasterRepository.GetMany(itemct => itemct.Status == "Active");
            return list;
        }

        public DesignationMaster GetId(int id)
        {
            var idlist = _DesignationMasterRepository.GetById(id);
            return idlist;
        }
    }
}
