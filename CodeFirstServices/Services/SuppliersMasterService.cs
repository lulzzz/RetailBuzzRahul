using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class SuppliersMasterService : ISuppliersMasterService
    {
        private readonly ISuppliersMasterRepository _SupplierMasterRepository;
        private readonly IUnitOfWork _unityOfWork;
        public SuppliersMasterService(ISuppliersMasterRepository SupplierMasterRepository, IUnitOfWork unityOfWork)
        {
            this._SupplierMasterRepository = SupplierMasterRepository;
            this._unityOfWork = unityOfWork;

        }

        public IEnumerable<SupplierMaster> getAllSuppliers()
        {
            var supplier = _SupplierMasterRepository.GetAll();
            return supplier;
        }

        public SupplierMaster getLastInsertedSupplier()
        {
            var supplier = _SupplierMasterRepository.GetAll().LastOrDefault();
            return supplier;
        }

        public SupplierMaster getById(int id)
        {
            var supplier = _SupplierMasterRepository.GetById(id);
            return supplier;
        }

        public void CreateSupplier(SupplierMaster Supplier)
        {
            _SupplierMasterRepository.Add(Supplier);
            _unityOfWork.Commit();
        }

        public void UpdateSupplier(SupplierMaster Supplier)
        {
            _SupplierMasterRepository.Update(Supplier);
            _unityOfWork.Commit();
        }

        public IEnumerable<SupplierMaster> GetEmail(string mail)
        {
            var suppilerList = _SupplierMasterRepository.GetMany(sp => sp.Email == mail);
            return suppilerList;
        }

        public IEnumerable<SupplierMaster> GetSupplierNames(string name)
        {
            var namelist = _SupplierMasterRepository.GetMany(suppname => suppname.SupplierName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(supname => supname.SupplierName);
            return namelist;
        }

        public IEnumerable<SupplierMaster> ValidateName(string suppliername)
        {
            var suppnamelist = _SupplierMasterRepository.GetMany(sp => sp.SupplierName == suppliername);
            return suppnamelist;
        }

        public SupplierMaster GetByName(string name)
        {
            var details = _SupplierMasterRepository.Get(supp => supp.SupplierName == name);
            return details;
        }

        public IEnumerable<SupplierMaster> LoadSupplierNameBySupplierType(string name)
        {
            var list = _SupplierMasterRepository.GetMany(x => x.SupplierType.Contains(name) && x.Status == "Active").ToList();
            return list;
        }

        public void DeleteSupplier(SupplierMaster Supplier)
        {
            _SupplierMasterRepository.Delete(Supplier);
            _unityOfWork.Commit();
        }

        public IEnumerable<SupplierMaster> GetActiveSuppliers()
        {
            var details = _SupplierMasterRepository.GetMany(supp => supp.Status == "Active");
            return details;
        }

        public IEnumerable<SupplierMaster> GetActiveSupplersByName(string name)
        {
            var list = _SupplierMasterRepository.GetMany(s => s.SupplierName.ToLower().StartsWith(name.ToLower()) && s.Status == "Active").OrderBy(s => s.SupplierName);
            return list;
        }
    }
}
