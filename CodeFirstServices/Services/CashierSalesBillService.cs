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
    public class CashierSalesBillService : ICashierSalesBillService
    {
        private readonly ICashierSalesBillRepository _cashiersalesbillrepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierSalesBillService(ICashierSalesBillRepository cashiersalesbillrepository, IUnitOfWork unitOfWork)
        {
            this._cashiersalesbillrepository = cashiersalesbillrepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierSalesBill cash)
        {
            _cashiersalesbillrepository.Add(cash);
            _unitOfWork.Commit();
        }

        public void Update(CashierSalesBill cash)
        {
            _cashiersalesbillrepository.Update(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierSalesBill> GetRowsBySBNo(string billno)
        {
            var data = _cashiersalesbillrepository.GetMany(s => s.SalesBillNo == billno);
            return data;
        }

        public IEnumerable<CashierSalesBill> GetBillsByHandoverStatus()
        {
            var data = _cashiersalesbillrepository.GetMany(s => s.HandoverStatus == "Active");
            return data;
        }

        public CashierSalesBill GetDetailsByCashierCode(string cashcode)
        {
            var data = _cashiersalesbillrepository.Get(d => d.CashierCode == cashcode);
            return data;
        }

        public CashierSalesBill GetRowsBySBAndCashierNo(string salesno, string cashierno)
        {
            var data = _cashiersalesbillrepository.Get(s => s.SalesBillNo == salesno && s.CashierCode == cashierno);
            return data;
        }

        public IEnumerable<CashierSalesBill> GetBillsByDate(DateTime date)
        {
            var data = _cashiersalesbillrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public CashierSalesBill GetDetailsBySBNo(string salesno)
        {
            var data = _cashiersalesbillrepository.Get(s => s.SalesBillNo == salesno);
            return data;
        }

        public IEnumerable<CashierSalesBill> GetAll()
        {
            var data = _cashiersalesbillrepository.GetAll();
            return data;
        }

        public IEnumerable<CashierSalesBill> GetDailyCashierSalesBill()
        {
            var list = _cashiersalesbillrepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<CashierSalesBill> GetActivecashierSalesbill(string salesbill)
        {
            var data = _cashiersalesbillrepository.GetMany(s => s.SalesBillNo == salesbill && s.Status == "Active");
            return data;
        }

        public IEnumerable<CashierSalesBill> GetHandoverSalesBillCashier()
        {
            var data = _cashiersalesbillrepository.GetMany(s => (s.Status == "InActive") && (s.HandoverCreditAmt != 0 || s.HandoverDebitAmt != 0 || s.HandoverChequeAmt != 0));
            return data;
        }

        public IEnumerable<CashierSalesBill> GetRowsByDate(string salesbillno, DateTime date)
        {
            var data = _cashiersalesbillrepository.GetMany(b => Convert.ToDateTime(b.Date).Date == date && b.Status == "Active" && b.SalesBillNo.ToLower().Contains(salesbillno.ToString().ToLower())).OrderBy(b => b.SalesBillNo);
            return data;
        }

        public CashierSalesBill GetDataByStatusAndSalesBillNo(string billno)
        {
            var data = _cashiersalesbillrepository.Get(s => s.Status == "Active" && s.SalesBillNo == billno);
            return data;
        }
    }
}
