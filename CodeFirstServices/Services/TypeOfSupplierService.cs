using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class TypeOfSupplierService : ITypeOfSupplierService
    {
        private readonly ITypeOfSupplierRepository _TypeOfSupplierRepository;
        private readonly IUnitOfWork _UnityOfWork;

        public TypeOfSupplierService(ITypeOfSupplierRepository TypeOfSuuplierRepository, IUnitOfWork UnityOfWork)
        {
            this._TypeOfSupplierRepository = TypeOfSuuplierRepository;
            this._UnityOfWork = UnityOfWork;
        }


        public IEnumerable<TypeOfSupplier> GetTypeOfSuppliers()
        {
            var typeOfSupplier = _TypeOfSupplierRepository.GetAll();
            return typeOfSupplier;
        }

        public string GetTypeOfSupplier(int id)
        {
            var typeOfSupplier = _TypeOfSupplierRepository.Get(sp => sp.TypeOfSupplierId == id);
            return typeOfSupplier.type_supplier; 
        }

        public TypeOfSupplier GetTypeOfsupplierById(int id)
        {
            var typeOfSupplier = _TypeOfSupplierRepository.GetById(id);
            return typeOfSupplier;
        
        }
    }
}
