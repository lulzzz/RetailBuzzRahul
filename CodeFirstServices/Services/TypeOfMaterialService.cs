using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using System.Linq;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
    public class TypeOfMaterialService : ITypeOfMaterialService
    {
        private readonly ITypeOfMaterialRepository _typeOfMaterialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TypeOfMaterialService(ITypeOfMaterialRepository typeOfMaterialRepository, IUnitOfWork unitOfWork)
        {
            this._typeOfMaterialRepository = typeOfMaterialRepository;
            this._unitOfWork = unitOfWork;
        }
               
        public void AddTypeOfMaterial(TypeOfMaterial typeOfMaterial)
        {
            _typeOfMaterialRepository.Add(typeOfMaterial);
            _unitOfWork.Commit();
        }

        public IEnumerable<TypeOfMaterial> GetMaterialList()
        {
            var list = _typeOfMaterialRepository.GetMany(m => m.Status == "Active");
            return list;
        }
        
        public IEnumerable<TypeOfMaterial> GetMaterialforList(string Material)
        {
            var list = _typeOfMaterialRepository.GetMany(m => m.MaterialName.ToLower().StartsWith(Material.ToString().ToLower()) && m.Status == "Active");
            return list;
        } 
  
        public TypeOfMaterial GetMaterialDetailById(int id)
        {
            var Id = _typeOfMaterialRepository.Get(list => list.MaterialId==id);
            return Id;
        }
        public TypeOfMaterial GetMaterialLast()
        {
            var material = _typeOfMaterialRepository.GetAll().LastOrDefault();
            return material;
        }

        public void UpdateTypeOfMaterial(TypeOfMaterial typeOfMaterial)
        {
            _typeOfMaterialRepository.Update(typeOfMaterial);
            _unitOfWork.Commit();
        }

        public IEnumerable<TypeOfMaterial> getAllTypeOfMaterial()
        {
            var typeOfMaterial = _typeOfMaterialRepository.GetAll();
            return typeOfMaterial;
        }

        public TypeOfMaterial GetTypeOfMaterialByCode(string code)
        {
            var typeData = _typeOfMaterialRepository.Get(ty => ty.MaterialShortName == code);
            return typeData;
        }

        public string GetNameByShortName(string shortname)
        {
            var data = _typeOfMaterialRepository.Get(ty => ty.MaterialShortName == shortname);
            return data.MaterialName;
        }

        public IEnumerable<TypeOfMaterial> GetMaterialNames(string name)
        {
            var namelist = _typeOfMaterialRepository.GetMany(n => n.MaterialName == name);
            return namelist;
        }

        public TypeOfMaterial GetMaterialName(string name)
        {
            var data = _typeOfMaterialRepository.Get(ty => ty.MaterialName == name);
            return data;
        }
    }
}
