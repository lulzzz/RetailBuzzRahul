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
    public class BillPaymentItemService : IBillPaymentItemService
    {
        private readonly IBillPaymentItemRepository _BillPaymentItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BillPaymentItemService(IBillPaymentItemRepository BillPaymentItemRepository, IUnitOfWork unitOfWork)
        {
            this._BillPaymentItemRepository = BillPaymentItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(BillPaymentItem cash)
        {
            _BillPaymentItemRepository.Add(cash);
            _unitOfWork.Commit();
        }
    }
}
