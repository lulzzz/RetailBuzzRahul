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
    public class CashierTemporaryCashMemoService:ICashierTemporaryCashMemoService
    {
        private readonly ICashierTemporaryCashMemoRepository _CashierTemporaryCashMemoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierTemporaryCashMemoService(ICashierTemporaryCashMemoRepository CashierTemporaryCashMemoRepository, IUnitOfWork unitOfWork)
        {
            this._CashierTemporaryCashMemoRepository = CashierTemporaryCashMemoRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierTemporaryCashMemo cash)
        {
            _CashierTemporaryCashMemoRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public void Update(CashierTemporaryCashMemo cash)
        {
            _CashierTemporaryCashMemoRepository.Update(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierTemporaryCashMemo> GetRowsByRBNo(string billno)
        {
            var data = _CashierTemporaryCashMemoRepository.GetMany(s => s.TempCashMemoNo == billno);
            return data;
        }

        public CashierTemporaryCashMemo GetDetailsByTempCashMemoNo(string tempcashmemono)
        {
            var details = _CashierTemporaryCashMemoRepository.Get(g => g.TempCashMemoNo == tempcashmemono);
            return details;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetBillsByHandoverStatus()
        {
            var data = _CashierTemporaryCashMemoRepository.GetMany(s => s.HandoverStatus == "Active");
            return data;
        }

        public CashierTemporaryCashMemo GetRowsByTCMAndCashierNo(string tempcashmemono, string cashierno)
        {
            var data = _CashierTemporaryCashMemoRepository.Get(s => s.TempCashMemoNo == tempcashmemono && s.CashierCode == cashierno);
            return data;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetBillsByDateAndTempCashMemoNo(DateTime date, string billno)
        {
            var data = _CashierTemporaryCashMemoRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.TempCashMemoNo == billno);
            return data;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetAll()
        {
            var list = _CashierTemporaryCashMemoRepository.GetAll();
            return list;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetDailyCashierTempCashMemo()
        {
            var list = _CashierTemporaryCashMemoRepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetActivecashierTemporaryCashMemo(string orderno)
        {
            var data = _CashierTemporaryCashMemoRepository.GetMany(s => s.TempCashMemoNo == orderno && s.Status == "Active");
            return data;
        }

        public IEnumerable<CashierTemporaryCashMemo> GetHandoverTemporaryCashMemoService()
        {
            var data = _CashierTemporaryCashMemoRepository.GetMany(s => (s.Status == "InActive") && (s.HandoverCreditAmt != 0 || s.HandoverDebitAmt != 0 || s.HandoverChequeAmt != 0));
            return data;
        }
    }
}
