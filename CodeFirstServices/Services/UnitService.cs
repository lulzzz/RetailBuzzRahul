using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository
            _iunitmasterrepository;
        private readonly IUnitOfWork _unitofwork;

        public UnitService(IUnitRepository iunitmasterrepository, IUnitOfWork unitofwork)
        {
            this._iunitmasterrepository = iunitmasterrepository;
            this._unitofwork = unitofwork;
        }

        public void createunit(UnitMaster unitmaster)
        {
            _iunitmasterrepository.Add(unitmaster);
            _unitofwork.Commit();
        }

        public void updateunit(UnitMaster unitmaster)
        {
            _iunitmasterrepository.Update(unitmaster);
            _unitofwork.Commit();
        }
        public UnitMaster getLastInserted()
        {
            var lastinserted = _iunitmasterrepository.GetAll().LastOrDefault();
            return lastinserted;
        }
        public UnitMaster getBycode(string code)
        {
            var item = _iunitmasterrepository.Get(it => it.UnitCode == code && it.status == "Active");
            return item;
        }

        public UnitMaster getById(int id)
        {
            var sizeid = _iunitmasterrepository.GetById(id);
            return sizeid;
        }

        public IEnumerable<UnitMaster> getallsize()
        {
            var sizecode = _iunitmasterrepository.GetMany(it => it.status == "Active");
            return sizecode;
        }

        public string GetNumberTypeByUnit(string unit)
        {
            string numbertype = _iunitmasterrepository.Get(m => m.UnitName == unit).NumberType;
            return numbertype;
        }


        public UnitMaster GetDetailsByName(string Name)
        {
            var details = _iunitmasterrepository.Get(u => u.UnitName == Name);
            return details;
        }

        public IEnumerable<UnitMaster> GetUnitList(string term)
        {
            var list = _iunitmasterrepository.GetMany(l1 => l1.UnitName.ToLower().StartsWith(term.ToString().ToLower())).OrderBy(s => s.UnitName);
            return list;
        }
    }
}
