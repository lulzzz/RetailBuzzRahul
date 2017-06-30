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
    public class ResetRetailBillAdjAmtDetailService : IResetRetailBillAdjAmtDetailService
    {
        private readonly IResetRetailBillAdjAmtDetailRepository _RetailBillAdjAmtDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ResetRetailBillAdjAmtDetailService(IResetRetailBillAdjAmtDetailRepository RetailBillAdjAmtDetailRepository, IUnitOfWork unitOfWork)
        {
            this._RetailBillAdjAmtDetailRepository = RetailBillAdjAmtDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(ResetRetailBillAdjAmtDetail data)
        {
            _RetailBillAdjAmtDetailRepository.Add(data);
            _unitOfWork.Commit();
        }
    }
}
