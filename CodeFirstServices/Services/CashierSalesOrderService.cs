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
    public class CashierSalesOrderService:ICashierSalesOrderService
    {
        private readonly ICashierSalesOrderRepository _cashiersalesorderrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetCashRowsBySORepository _GetCashRowsBySO;
        public CashierSalesOrderService(ICashierSalesOrderRepository cashiersalesorderrepository, IUnitOfWork unitOfWork, IGetCashRowsBySORepository GetCashRowsBySO)
        {
            this._cashiersalesorderrepository = cashiersalesorderrepository;
            this._GetCashRowsBySO = GetCashRowsBySO;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierSalesOrder cash)
        {
            _cashiersalesorderrepository.Add(cash);
            _unitOfWork.Commit();
        }

        public void Update(CashierSalesOrder cash)
        {
            _cashiersalesorderrepository.Update(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<GetCashRowsBySO> GetRowsByOrderNo(string procname)
        {
            var data = _GetCashRowsBySO.ExecuteCustomStoredProc(procname);
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetOrderNoByDate(DateTime date)
        {
            var data = _cashiersalesorderrepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == date);
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetRowsByDate(string orderno, DateTime date)
        {
            var data = _cashiersalesorderrepository.GetMany(b => Convert.ToDateTime(b.CashierDate).Date == date && b.Status == "Active" && b.OrderNo.ToLower().Contains(orderno.ToString().ToLower())).OrderBy(b => b.OrderNo);
            return data;
        }

        public CashierSalesOrder GetDetailsBySONo(string orderno)
        {
            var data = _cashiersalesorderrepository.Get(s => s.OrderNo == orderno);
            return data;
        }

        public CashierSalesOrder GetDetailsByCashierCode(string code)
        {
            var data = _cashiersalesorderrepository.Get(s => s.CashierCode == code);
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetSalesOrderNos(string order)
        {
            var data = _cashiersalesorderrepository.GetMany(b => b.OrderNo.ToLower().Contains(order.ToString().ToLower()) && b.Status == "Active").OrderBy(b => b.OrderNo);
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetRowsBySONo(string orderno)
        {
            var data = _cashiersalesorderrepository.GetMany(s => s.OrderNo == orderno);
            return data;
        }

        public CashierSalesOrder GetDetailsBySONoAndStatus(string orderno)
        {
            var data = _cashiersalesorderrepository.Get(s => s.OrderNo == orderno && s.Status=="Active");
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetBillsByHandoverStatus()
        {
            var data = _cashiersalesorderrepository.GetMany(s => s.HandoverStatus == "Active");
            return data;
        }

        public CashierSalesOrder GetRowsBySOAndCashierNo(string orderno,string cashierno)
        {
            var data = _cashiersalesorderrepository.Get(s => s.OrderNo == orderno && s.CashierCode == cashierno);
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCard(DateTime date)
        {
            var data = _cashiersalesorderrepository.GetMany(s => Convert.ToDateTime(s.CashierDate).Date == date.Date && (s.PaymentType == "Card" || s.PaymentType == "CashCardCheque"));
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCash(DateTime date)
        {
            var data = _cashiersalesorderrepository.GetMany(s => Convert.ToDateTime(s.CashierDate).Date == date.Date && (s.PaymentType == "Cash" || s.PaymentType == "CashCardCheque"));
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCheque(DateTime date)
        {
            var data = _cashiersalesorderrepository.GetMany(s => Convert.ToDateTime(s.CashierDate).Date == date.Date && (s.PaymentType == "Cheque" || s.PaymentType == "CashCardCheque"));
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetAll()
        {
            var data = _cashiersalesorderrepository.GetAll();
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetDailyCashierSalesOrder()
        {
            var list = _cashiersalesorderrepository.GetMany(c => Convert.ToDateTime(c.CashierDate).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<CashierSalesOrder> GetActivecashierSalesorder(string orderno)
        {
            var data = _cashiersalesorderrepository.GetMany(s => s.OrderNo == orderno && s.Status == "Active");
            return data;
        }

        public IEnumerable<CashierSalesOrder> GetHandoverCashiers()
        {
            var data = _cashiersalesorderrepository.GetMany(s => (s.Status == "InActive") && (s.HandoverCreditAmt != 0 || s.HandoverDebitAmt != 0 || s.HandoverChequeAmt != 0));
            return data;
        }
    }
}
