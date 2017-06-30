using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
  public  class LabMasterService:ILabMasterService
   {
      private readonly ILabRepository _labRepository;
      private readonly IUnitOfWork _unitOfWork;

      public LabMasterService(ILabRepository labrepository, IUnitOfWork unitofwork)
      {
          this._labRepository = labrepository;
          this._unitOfWork = unitofwork;
      }

      public IEnumerable<LabMaster> getAllLabs()
      {
          var lab1 = _labRepository.GetMany(lab=>lab.labStatus=="Active");
          return lab1;
      }
      public LabMaster getByLabCode(string code)
      {
          var item = _labRepository.Get(it => it.labCode == code && it.labStatus == "Active");
          return item;
      }

      public void createLab(LabMaster labmaster)
      {
          _labRepository.Add(labmaster);
          _unitOfWork.Commit();
      }
      //public ItemMaster GetLastCategory()
      //{
      //    var lab = _labRepository.GetAll().LastOrDefault();
      //    return lab;
      //}

      public void updateLab(LabMaster labmaster)
      {
          _labRepository.Update(labmaster);
          _unitOfWork.Commit();
      }

      public  LabMaster getById(int id)
      {

          var labid = _labRepository.GetById(id);
          return labid;


      }

      public LabMaster getLastInserted()
      {
          var lastinserted = _labRepository.GetAll().LastOrDefault();
          return lastinserted;
      }










    }
}
