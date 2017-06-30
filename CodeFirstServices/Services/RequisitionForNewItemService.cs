using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
  public class RequisitionForNewItemService:IRequisitionForNewItemService
    {
       private readonly IRequisitionForNewItemRepository _RequisitionForNewItemRepository;
       private readonly IUnitOfWork _unityOfWork;


       public RequisitionForNewItemService(IRequisitionForNewItemRepository RequisitionForNewItemRepository, IUnitOfWork unityOfWork)
       {
           this._RequisitionForNewItemRepository = RequisitionForNewItemRepository;
           this._unityOfWork = unityOfWork;

       }


       public void createNewRequisition(RequisitionofNewItemsForShop req)
       {
           _RequisitionForNewItemRepository.Add(req);
           _unityOfWork.Commit();

       }

    }
}
