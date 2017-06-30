using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;


namespace CodeFirstServices.Services
{
 public class RequisitionOfPoService:IRequisitionOfPoService
    {
      private readonly IRequisitionOfPoRepository _reqRepository;
      private readonly IUnitOfWork _unitOfWork;

      public RequisitionOfPoService(IRequisitionOfPoRepository reqRepository, IUnitOfWork unitOfWork)
        {
            this._reqRepository = reqRepository;
            this._unitOfWork = unitOfWork;
        }

      public void createRequisitionOfPo(RequisitionOfPo req)
      {
          _reqRepository.Add(req);
          _unitOfWork.Commit();

      }

    }
}
