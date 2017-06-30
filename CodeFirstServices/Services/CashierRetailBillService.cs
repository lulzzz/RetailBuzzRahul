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
    public class CashierRetailBillService:ICashierRetailBillService
    {
        private readonly ICashierRetailBillRepository _cashierretailbillrepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierRetailBillService(ICashierRetailBillRepository cashierretailbillrepository, IUnitOfWork unitOfWork)
        {
            this._cashierretailbillrepository = cashierretailbillrepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierRetailBill cash)
        {
            _cashierretailbillrepository.Add(cash);
            _unitOfWork.Commit();
        }

        public void Update(CashierRetailBill cash)
        {
            _cashierretailbillrepository.Update(cash);
            _unitOfWork.Commit();
        }

        public CashierRetailBill GetDetailsByRetailNo(string retailno)
        {
            var details = _cashierretailbillrepository.Get(g => g.RetailBillNo == retailno);
            return details;
        }

        public IEnumerable<CashierRetailBill> GetRowsByRBNo(string billno)
        {
            var data = _cashierretailbillrepository.GetMany(s => s.RetailBillNo == billno);
            return data;
        }

        public IEnumerable<CashierRetailBill> GetBillsByHandoverStatus()
        {
            var data = _cashierretailbillrepository.GetMany(s => s.HandoverStatus == "Active");
            return data;
        }

        public CashierRetailBill GetRowsByRIAndCashierNo(string retailno, string cashierno)
        {
            var data = _cashierretailbillrepository.Get(s => s.RetailBillNo == retailno && s.CashierCode == cashierno);
            return data;
        }

        public IEnumerable<CashierRetailBill> GetBillsByDate(DateTime date)
        {
            var data = _cashierretailbillrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public IEnumerable<CashierRetailBill> GetBillsByDateAndDateStatus(DateTime date)
        {
            var data = _cashierretailbillrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.BillDateStatus == "Different");
            return data;
        }

        public IEnumerable<CashierRetailBill> GetBillsByDateAndRetailBillNo(DateTime date, string billno)
        {
            var data = _cashierretailbillrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.RetailBillNo == billno);
            return data;
        }

        public CashierRetailBill GetDataByStatusAndRetailBillNo(string billno)
        {
            var data = _cashierretailbillrepository.Get(s => s.Status == "Active" && s.RetailBillNo == billno);
            return data;
        }

        public IEnumerable<CashierRetailBill> GetAll()
        {
            var data = _cashierretailbillrepository.GetAll();
            return data;
        }

        public IEnumerable<CashierRetailBill> GetDailyCashierRetailBill()
        {
            var list = _cashierretailbillrepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<CashierRetailBill> GetActivecashierRetailbill(string orderno)
        {
            var data = _cashierretailbillrepository.GetMany(s => s.RetailBillNo == orderno && s.Status == "Active");
            return data;
        }

        public IEnumerable<CashierRetailBill> GetHandoverRetailCashier()
        {
            var data = _cashierretailbillrepository.GetMany(s => (s.Status == "InActive") && (s.HandoverCreditAmt != 0 || s.HandoverDebitAmt != 0 || s.HandoverChequeAmt != 0));
            return data;
        }

        public IEnumerable<CashierRetailBill> GetRowsByDate(string retailbillno, DateTime date)
        {
            var data = _cashierretailbillrepository.GetMany(b => Convert.ToDateTime(b.Date).Date == date && b.Status == "Active" && b.RetailBillNo.ToLower().Contains(retailbillno.ToString().ToLower())).OrderBy(b => b.RetailBillNo);
            return data;
        }
    }
}
