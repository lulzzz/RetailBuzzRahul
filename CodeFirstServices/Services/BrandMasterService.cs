using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class BrandMasterService : IBrandMasterService
    {
        private readonly IBrandMasterRepository
            _ibrandmasterrepository;
        private readonly IUnitOfWork _unitofwork;

        public BrandMasterService(IBrandMasterRepository ibrandmasterrepository, IUnitOfWork unitofwork)
        {
            this._ibrandmasterrepository = ibrandmasterrepository;
            this._unitofwork = unitofwork;
        }

        public void Create(BrandMaster brandmaster)
        {
            _ibrandmasterrepository.Add(brandmaster);
            _unitofwork.Commit();
        }

        public void Update(BrandMaster brandmaster)
        {
            _ibrandmasterrepository.Update(brandmaster);
            _unitofwork.Commit();
        }

        public BrandMaster getLastBrand()
        {
            var lastrow = _ibrandmasterrepository.GetAll().LastOrDefault();
            return lastrow;
        }

        public IEnumerable<BrandMaster> GetBrandList(string name)
        {
            var list = _ibrandmasterrepository.GetMany(l1 => l1.BrandName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.BrandName);
            return list;
        }

        public IEnumerable<BrandMaster> GetBrandName(string name)
        {
            var namelist = _ibrandmasterrepository.GetMany(n => n.BrandName == name);
            return namelist;
        }

        public BrandMaster getById(int id)
        {
            var data = _ibrandmasterrepository.Get(it => it.Id == id);
            return data;
        }

        public IEnumerable<BrandMaster> GetBrands()
        {
            var list = _ibrandmasterrepository.GetMany(itemct => itemct.Status == "Active");
            return list;
        }

        public BrandMaster GetId(int id)
        {
            var idlist = _ibrandmasterrepository.GetById(id);
            return idlist;
        }

        public IEnumerable<BrandMaster> GetAllBrands()
        {
            var data = _ibrandmasterrepository.GetAll();
            return data;
        }

        public BrandMaster brandnamebyname(string brandname)
        {
            var data = _ibrandmasterrepository.Get(bn => bn.BrandName == brandname);
            return data;
        }
    }
}
