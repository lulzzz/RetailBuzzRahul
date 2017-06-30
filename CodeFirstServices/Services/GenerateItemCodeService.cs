using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
  public  class GenerateItemCodeService:IGenerateItemCodeService
    {
      private readonly IGenerateItemCodeRepository _GenerateItemCodeRepository;
      private readonly IUnitOfWork _UnitOfWork;

      public GenerateItemCodeService(IGenerateItemCodeRepository GenerateItemCodeRepository, IUnitOfWork UnitOfWork)
      {
          this._GenerateItemCodeRepository= GenerateItemCodeRepository;
          this._UnitOfWork = UnitOfWork;

      }



      public void createItemCode(GenerateItemCode generateItemCode)
      {

          _GenerateItemCodeRepository.Add(generateItemCode);
          _UnitOfWork.Commit();
          
      }

      public void updateItemCode(GenerateItemCode generateItemCode)
      {
          _GenerateItemCodeRepository.Update(generateItemCode);
          _UnitOfWork.Commit();
      }

      public GenerateItemCode getbycount(string shortName, int Count)
      {
          var code = _GenerateItemCodeRepository.Get(cd=>cd.categoryshortname==shortName && cd.count==Count);
          return code;
   
      }
     public  GenerateItemCode getbyshortName(string shortName)
     {
         var shname = _GenerateItemCodeRepository.Get(ad => ad.categoryshortname == shortName);
         return shname;
     }

    }
}
