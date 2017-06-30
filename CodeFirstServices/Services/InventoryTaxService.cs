using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class InventoryTaxService : IInventoryTaxService
    {
        private readonly IInventoryTaxRepository _InventoryTaxRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public InventoryTaxService(IInventoryTaxRepository InventoryTaxRepository, IUnitOfWork UnitOfWork)
        {
            this._InventoryTaxRepository = InventoryTaxRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(InventoryTax InventoryTax)
        {
            _InventoryTaxRepository.Add(InventoryTax);
            _UnitOfWork.Commit();
        }

        public IEnumerable<InventoryTax> GetTaxesByCode(string code)
        {
            var data = _InventoryTaxRepository.GetMany(m => m.Code == code);
            return data;
        }

        public void Update(InventoryTax InventoryTax)
        {
            _InventoryTaxRepository.Update(InventoryTax);
            _UnitOfWork.Commit();
        }

        public void Delete(InventoryTax InventoryTax)
        {
            _InventoryTaxRepository.Delete(InventoryTax);
            _UnitOfWork.Commit();
        }

        public InventoryTax GetTaxById(int id)
        {
            var data = _InventoryTaxRepository.GetById(id);
            return data;
        }
    }
}
