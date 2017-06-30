using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class DesignService : IDesignService
    {
        private readonly IDesignReposirory _designRepository;
        private readonly IUnitOfWork _unitofwork;

        public DesignService(IDesignReposirory itemdesignmasterRepository, IUnitOfWork unitofwork)
        {
            this._designRepository = itemdesignmasterRepository;
            this._unitofwork = unitofwork;
        }

        public void CreateDesign(DesignMaster itemdesign)
        {
            _designRepository.Add(itemdesign);
            _unitofwork.Commit();
        }

        public void UpdateDesign(DesignMaster itemdesign)
        {
            _designRepository.Update(itemdesign);
            _unitofwork.Commit();
        }

        public DesignMaster GetLastDesignRow()
        {
            var lastrow = _designRepository.GetAll().LastOrDefault();
            return lastrow;
        }

        public DesignMaster GetDetailsById(int Id)
        {
            var details = _designRepository.GetById(Id);
            return details;
        }


        public IEnumerable<DesignMaster> GetDesignNamesBySubCat(string type)
        {
            var list = _designRepository.GetMany(l => l.SubCategoryName == type && l.Status == "Active");
            return list;
        }

        public DesignMaster GetById(int id)
        {
            var data = _designRepository.GetById(id);
            return data;
        }

        public int getDesignsByDesignCode(string code)
        {
            var designData = _designRepository.GetMany(dc => dc.DesignCode == code);
            return designData.Count();
        }



        public string getNameByCode(string code)
        {
            var data = _designRepository.Get(dc => dc.DesignCode == code);
            return data.DesignName;
        }


        public DesignMaster GetDetailsByCode(string code)
        {
            var data = _designRepository.Get(dc => dc.DesignCode == code);
            return data;
        }

        public IEnumerable<DesignMaster> GetAll()
        {
            var details = _designRepository.GetAll();
            return details;
        }

        public IEnumerable<DesignMaster> GetDesignName(string term)
        {
            var details = _designRepository.GetMany(d => d.DesignName.ToLower().ToString().StartsWith(term.ToString().ToLower()));
            return details;
        }

        public DesignMaster GetNamebyDesignName(string name)
        {
            var data = _designRepository.Get(dc => dc.DesignName == name);
            return data;
        }
    }
}
