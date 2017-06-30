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
    public class BillPaymentService : IBillPaymentService
    {
        private readonly IBillPaymentRepository _BillPaymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BillPaymentService(IBillPaymentRepository BillPaymentRepository, IUnitOfWork unitOfWork)
        {
            this._BillPaymentRepository = BillPaymentRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(BillPayment cash)
        {
            _BillPaymentRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public BillPayment GetLastRow()
        {
            var row = _BillPaymentRepository.GetAll().LastOrDefault();
            return row;
        }
    }
}
