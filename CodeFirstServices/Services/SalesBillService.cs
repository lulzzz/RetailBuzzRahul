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
    public class SalesBillService : ISalesBillService
    {
        private readonly ISalesBillRepository _isalesbilldetailrepository;
        private readonly IUnitOfWork _iunitofwork;

        public SalesBillService(ISalesBillRepository salesbilldetailrep, IUnitOfWork unitofwork)
        {
            this._isalesbilldetailrepository = salesbilldetailrep;
            this._iunitofwork = unitofwork;
        }

        public void Create(SalesBill bill)
        {
            _isalesbilldetailrepository.Add(bill);
            _iunitofwork.Commit();
        }

        public void Update(SalesBill bill)
        {
            _isalesbilldetailrepository.Update(bill);
            _iunitofwork.Commit();
        }

        public SalesBill GetLastSalesBillByFinYr(string year, string shopcode)
        {
            var data = _isalesbilldetailrepository.GetMany(s => s.SalesBillNo.Contains(year) && s.ShopCode == shopcode).OrderBy(s => s.SalesBillNo).LastOrDefault();
            return data;
        }

        public SalesBill GetDetailsById(int id)
        {
            var details = _isalesbilldetailrepository.GetById(id);
            return details;
        }

        public IEnumerable<SalesBill> GetAll()
        {
            var row = _isalesbilldetailrepository.GetAll();
            return row;
        }

        public IEnumerable<SalesBill> GetSalesBillNos(string billno)
        {
            var data = _isalesbilldetailrepository.GetMany(b => b.SalesBillNo.ToLower().Contains(billno.ToString().ToLower()) && b.Status == "Active").OrderBy(b => b.SalesBillNo);
            return data;
        }

        public SalesBill GetDetailsBySalesBillNo(string no)
        {
            var data = _isalesbilldetailrepository.Get(n => n.SalesBillNo == no);
            return data;
        }

        public IEnumerable<SalesBill> GetSalesBillByDate()
        {
            var data = _isalesbilldetailrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<SalesBill> GetSalesBillByClient(string client)
        {
            var data = _isalesbilldetailrepository.GetMany(s => s.ClientName == client && s.CashierStatus == "Active");
            return data;
        }

        public IEnumerable<SalesBill> GetSalesBillNosByCashierStatus(string billno)
        {
            var data = _isalesbilldetailrepository.GetMany(b => b.SalesBillNo.ToLower().Contains(billno.ToString().ToLower()) && b.CashierStatus == "Active").OrderBy(b => b.SalesBillNo);
            return data;
        }

        public IEnumerable<SalesBill> GetBillsByDateAndAdjustAmount(DateTime date)
        {
            var data = _isalesbilldetailrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.AdjustedAmount != 0);
            return data;
        }

        public IEnumerable<SalesBill> GetBillsByClientName(string client)
        {
            var data = _isalesbilldetailrepository.GetMany(d => d.ClientName == client);
            return data;
        }

        public IEnumerable<SalesBill> GetSalesBillByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var list = _isalesbilldetailrepository.GetMany(s => Convert.ToDateTime(s.Date).Date >= fromdate.Date && Convert.ToDateTime(s.Date).Date <= todate.Date);
            return list;
        }

        public IEnumerable<SalesBill> GetSalesBillNosByWithTaxStatus(string term)
        {
            var list = _isalesbilldetailrepository.GetMany(s => s.SalesBillNo.ToLower().Contains(term.ToLower()) && s.TaxStatus == "WithTax").OrderBy(r => r.SalesBillNo);
            return list;
        }

        public IEnumerable<SalesBill> GetSalesBillNosByWithoutTaxStatus(string term)
        {
            var list = _isalesbilldetailrepository.GetMany(s => s.SalesBillNo.ToLower().Contains(term.ToLower()) && s.TaxStatus == "WithoutTax").OrderBy(r => r.SalesBillNo);
            return list;
        }

        public IEnumerable<SalesBill> GetAllActiveSalesBill()
        {
            var data = _isalesbilldetailrepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<SalesBill> GetAllSalesBill(string term)
        {
            var list = _isalesbilldetailrepository.GetMany(b => b.SalesBillNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(b => b.SalesBillNo);
            return list;
        }

        public IEnumerable<SalesBill> GetBillsByDate(DateTime date)
        {
            var data = _isalesbilldetailrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }
    }
}

