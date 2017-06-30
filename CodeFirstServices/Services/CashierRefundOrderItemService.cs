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
    public class CashierRefundOrderItemService:ICashierRefundOrderItemService
    {
        private readonly ICashierRefundOrderItemRepository _CashierRefundOrderItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierRefundOrderItemService(ICashierRefundOrderItemRepository CashierRefundOrderItemRepository, IUnitOfWork unitOfWork)
        {
            this._CashierRefundOrderItemRepository = CashierRefundOrderItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierRefundOrderItem data)
        {
            _CashierRefundOrderItemRepository.Add(data);
            _unitOfWork.Commit();
        }
    }
}
