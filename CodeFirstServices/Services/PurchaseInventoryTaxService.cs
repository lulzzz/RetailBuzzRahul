using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class PurchaseInventoryTaxService : IPurchaseInventoryTaxService
    {
        private readonly IPurchaseInventoryTaxRepository _PurchaseInventoryTaxRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public PurchaseInventoryTaxService(IPurchaseInventoryTaxRepository PurchaseInventoryTaxRepository, IUnitOfWork UnitOfWork)
        {
            this._PurchaseInventoryTaxRepository = PurchaseInventoryTaxRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(PurchaseInventoryTax PurchaseInventoryTax)
        {
            _PurchaseInventoryTaxRepository.Add(PurchaseInventoryTax);
            _UnitOfWork.Commit();
        }

        public IEnumerable<PurchaseInventoryTax> GetTaxesByCode(string code)
        {
            var data = _PurchaseInventoryTaxRepository.GetMany(m => m.Code == code);
            return data;
        }

        public void Update(PurchaseInventoryTax PurchaseInventoryTax)
        {
            _PurchaseInventoryTaxRepository.Update(PurchaseInventoryTax);
            _UnitOfWork.Commit();
        }

        public void Delete(PurchaseInventoryTax PurchaseInventoryTax)
        {
            _PurchaseInventoryTaxRepository.Delete(PurchaseInventoryTax);
            _UnitOfWork.Commit();
        }

        public PurchaseInventoryTax GetTaxById(int id)
        {
            var data = _PurchaseInventoryTaxRepository.GetById(id);
            return data;
        }
    }
}
