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
    public class JobWorkPaymentService:IJobWorkPaymentService
    {
        private readonly IJobWorkPaymentRepository _JobWorkPaymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobWorkPaymentService(IJobWorkPaymentRepository JobWorkPaymentRepository, IUnitOfWork unitOfWork)
        {
            this._JobWorkPaymentRepository = JobWorkPaymentRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(JobWorkPayment data)
        {
            _JobWorkPaymentRepository.Add(data);
            _unitOfWork.Commit();
        }

        public void Update(JobWorkPayment data)
        {
            _JobWorkPaymentRepository.Update(data);
            _unitOfWork.Commit();
        }

        public JobWorkPayment GetLastPaymentByFinYr(string year)
        {
            var data = _JobWorkPaymentRepository.GetMany(p => p.PaymentCode.Contains(year)).OrderBy(p => p.PaymentCode).LastOrDefault();
            return data;
        }

        public IEnumerable<JobWorkPayment> GetRowsByOutwardNo(string outwardno)
        {
            var data = _JobWorkPaymentRepository.GetMany(s => s.OutwardToTailorNo == outwardno);
            return data;
        }

        public IEnumerable<JobWorkPayment> GetBillsByHandoverStatus()
        {
            var data = _JobWorkPaymentRepository.GetMany(s => s.HandoverStatus == "Active");
            return data;
        }

        public JobWorkPayment GetRowsByOutwardToTailorAndPaymentNo(string outtotailorno, string paymentno)
        {
            var data = _JobWorkPaymentRepository.Get(s => s.OutwardToTailorNo == outtotailorno && s.PaymentCode == paymentno);
            return data;
        }

        public IEnumerable<JobWorkPayment> GetHandoverJobWorkPayment()
        {
            var data = _JobWorkPaymentRepository.GetMany(s => (s.HandoverCreditAmt != 0 || s.HandoverDebitAmt != 0 || s.HandoverChequeAmt != 0));
            return data;
        }

    }
}
