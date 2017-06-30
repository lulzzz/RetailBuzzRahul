using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class TemporaryCashMemoService : ITemporaryCashMemoService
    {
        private readonly ITemporaryCashMemoRepository _TemporaryCashMemoRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public TemporaryCashMemoService(ITemporaryCashMemoRepository TemporaryCashMemoRepository, IUnitOfWork UnitOfWork)
        {
            this._TemporaryCashMemoRepository = TemporaryCashMemoRepository;
            this._UnitOfWork = UnitOfWork;
        }
        public void Create(TemporaryCashMemo cashmemo)
        {
            _TemporaryCashMemoRepository.Add(cashmemo);
            _UnitOfWork.Commit();
        }

        public void Update(TemporaryCashMemo cashmemo)
        {
            _TemporaryCashMemoRepository.Update(cashmemo);
            _UnitOfWork.Commit();
        }

        public TemporaryCashMemo GetById(int id)
        {
            var data = _TemporaryCashMemoRepository.GetById(id);
            return data;
        }

        public TemporaryCashMemo GetLastInvoiceDetails()
        {
            var data = _TemporaryCashMemoRepository.GetAll().LastOrDefault();
            return data;
        }
        public IEnumerable<TemporaryCashMemo> GetAll()
        {
            var data = _TemporaryCashMemoRepository.GetAll();
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetRetailNos(string billno)
        {
            var data = _TemporaryCashMemoRepository.GetMany(b => b.TempCashMemoNo.ToLower().StartsWith(billno.ToString().ToLower()) && b.Status == "Active").OrderBy(b => b.TempCashMemoNo);
            return data;
        }

        public TemporaryCashMemo GetDetailsByInvoiceNo(string no)
        {
            var data = _TemporaryCashMemoRepository.Get(n => n.TempCashMemoNo == no);
            return data;
        }

        public TemporaryCashMemo GetDetailsById(int id)
        {
            var details = _TemporaryCashMemoRepository.GetById(id);
            return details;
        }

        public IEnumerable<TemporaryCashMemo> GetRetailBillByDate()
        {
            var data = _TemporaryCashMemoRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetRetailByStatus(string retail)
        {
            var data = _TemporaryCashMemoRepository.GetMany(b => b.Status != "InActive" && b.TempCashMemoNo.ToLower().StartsWith(retail.ToString().ToLower())).OrderBy(b => b.TempCashMemoNo);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetRetailByCashierStatus(string retail)
        {
            var data = _TemporaryCashMemoRepository.GetMany(b => b.CashierStatus == "Active" && b.TempCashMemoNo.ToLower().StartsWith(retail.ToString().ToLower())).OrderBy(b => b.TempCashMemoNo);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetReportByDate(DateTime FromDate, DateTime ToDate, string Emp)
        {
            var data = _TemporaryCashMemoRepository.GetMany(r => Convert.ToDateTime(r.Date).Date >= Convert.ToDateTime(FromDate.Date) && Convert.ToDateTime(r.Date) <= Convert.ToDateTime(ToDate.Date) && r.SalesPersonName == Emp);
            return data;
        }

        public TemporaryCashMemo GetSalesPersonByShopName(string ShopName)
        {
            var details = _TemporaryCashMemoRepository.Get(m => m.ShopName == ShopName);
            return details;
        }

        public IEnumerable<TemporaryCashMemo> GetBillsByDate(DateTime date)
        {
            var data = _TemporaryCashMemoRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetBillsByDateAndAdjustAmount(DateTime date)
        {
            var data = _TemporaryCashMemoRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.AdjustedAmount != 0);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetBillsByClientName(string client)
        {
            var data = _TemporaryCashMemoRepository.GetMany(d => d.ClientName == client);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetBillsByConvertStatus()
        {
            var data = _TemporaryCashMemoRepository.GetMany(d => d.Convertstatus == "Active");
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetDetailsByCashierAndConvertStatus(string retail)
        {
            var data = _TemporaryCashMemoRepository.GetMany(b => b.CashierStatus == "Active" && b.Convertstatus == "Active" && b.TempCashMemoNo.ToLower().Contains(retail.ToString().ToLower())).OrderBy(b => b.TempCashMemoNo);
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetBillsByDateAndConvertStatus(DateTime date)
        {
            var data = _TemporaryCashMemoRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.Convertstatus == "Active"); ;
            return data;
        }

        public TemporaryCashMemo GetLastTempCashMemoByFinYr(string year, string shopcode)
        {
            var data = _TemporaryCashMemoRepository.GetMany(o => o.TempCashMemoNo.Contains(year) && o.ShopCode == shopcode).OrderBy(o => o.TempCashMemoNo).LastOrDefault();
            return data;
        }

        public IEnumerable<TemporaryCashMemo> GetAllActiveTemporaryCashMemo()
        {
            var data = _TemporaryCashMemoRepository.GetMany(d => d.Status == "Active");
            return data;
        }
    }
}
