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
    public class RetailBillService : IRetailBillService
    {
        private readonly IRetailBillRepository _RetailInvoiceMasterRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public RetailBillService(IRetailBillRepository RetailInvoiceMasterRepository, IUnitOfWork UnitOfWork)
        {
            this._RetailInvoiceMasterRepository = RetailInvoiceMasterRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(RetailBill retailinvoicemaster)
        {
            _RetailInvoiceMasterRepository.Add(retailinvoicemaster);
            _UnitOfWork.Commit();
        }

        public void Update(RetailBill retailinvoicemaster)
        {
            _RetailInvoiceMasterRepository.Update(retailinvoicemaster);
            _UnitOfWork.Commit();
        }

        public RetailBill GetById(int id)
        {
            var data = _RetailInvoiceMasterRepository.GetById(id);
            return data;
        }
        public RetailBill GetLastInvoiceDetails()
        {
            var data = _RetailInvoiceMasterRepository.GetAll().LastOrDefault();
            return data;
        }
        public IEnumerable<RetailBill> GetAll()
        {
            var data = _RetailInvoiceMasterRepository.GetAll();
            return data;
        }

        public IEnumerable<RetailBill> GetRetailNos(string billno)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(b => b.RetailBillNo.ToLower().Contains(billno.ToString().ToLower()) && b.Status == "Active").OrderBy(b => b.RetailBillNo);
            return data;
        }

        public RetailBill GetDetailsByInvoiceNo(string no)
        {
            var data = _RetailInvoiceMasterRepository.Get(n => n.RetailBillNo == no);
            return data;
        }

        public RetailBill GetDetailsById(int id)
        {
            var details = _RetailInvoiceMasterRepository.GetById(id);
            return details;
        }

        public IEnumerable<RetailBill> GetRetailBillByDate()
        {
            var data = _RetailInvoiceMasterRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<RetailBill> GetRetailByStatus(string retail)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(b => b.Status != "InActive" && b.RetailBillNo.ToLower().StartsWith(retail.ToString().ToLower())).OrderBy(b => b.RetailBillNo);
            return data;
        }

        public IEnumerable<RetailBill> GetRetailByCashierStatus(string retail)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(b => b.CashierStatus == "Active" && b.RetailBillNo.ToLower().Contains(retail.ToString().ToLower())).OrderBy(b => b.RetailBillNo);
            return data;
        }

        public IEnumerable<RetailBill> GetReportByDate(DateTime FromDate, DateTime ToDate, string Emp)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(r => Convert.ToDateTime(r.Date).Date >= Convert.ToDateTime(FromDate.Date) && Convert.ToDateTime(r.Date) <= Convert.ToDateTime(ToDate.Date) && r.SalesPersonName == Emp);
            return data;
        }

        public RetailBill GetSalesPersonByShopName(string ShopName)
        {
            var details = _RetailInvoiceMasterRepository.Get(m => m.ShopName == ShopName);
            return details;
        }

        public IEnumerable<RetailBill> GetBillsByDate(DateTime date)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public IEnumerable<RetailBill> GetBillsByDateAndAdjustAmount(DateTime date)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date && s.AdjustedAmount != 0);
            return data;
        }

        public IEnumerable<RetailBill> GetBillsByClientName(string client)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(d => d.ClientName == client);
            return data;
        }

        public IEnumerable<RetailBill> GetAllRetailBillNos(string term)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(b => b.RetailBillNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(b => b.RetailBillNo);
            return data;
        }

        //public IEnumerable<RetailBill> GetRetailNoByStatus(string retail)
        //{
        //    var data = _RetailInvoiceMasterRepository.GetMany(b => b.Status == "Active" && b.CashierStatus == "Active" && b.RetailBillNo.ToLower().StartsWith(retail.ToString().ToLower())).OrderBy(b => b.RetailBillNo);
        //    return data;
        //}

        public IEnumerable<RetailBill> GetRetailBillByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var list = _RetailInvoiceMasterRepository.GetMany(r => Convert.ToDateTime(r.Date).Date >= fromdate.Date && Convert.ToDateTime(r.Date).Date <= todate.Date);
            return list;
        }

        public RetailBill GetLastRetailBillByFinYr(string year, string shopcode)
        {
            var data = _RetailInvoiceMasterRepository.GetMany(r => r.RetailBillNo.Contains(year) && r.ShopCode == shopcode).OrderBy(r => r.RetailBillNo).LastOrDefault();
            return data;
        }

        public IEnumerable<RetailBill> GetRetailBillNosByWithTaxStatus(string term, string taxstatus)
        {
            var list = _RetailInvoiceMasterRepository.GetMany(r => r.RetailBillNo.ToLower().Contains(term.ToLower()) && r.TaxStatus == "WithTax").OrderBy(r => r.RetailBillNo);
            return list;
        }

        public IEnumerable<RetailBill> GetRetailBillNosByWithoutTaxStatus(string term, string taxstatus)
        {
            var list = _RetailInvoiceMasterRepository.GetMany(r => r.RetailBillNo.ToLower().Contains(term.ToLower()) && r.TaxStatus == "WithoutTax").OrderBy(r => r.RetailBillNo);
            return list;
        }

        public IEnumerable<RetailBill> GetAllActiveRetailBill()
        {
            var data = _RetailInvoiceMasterRepository.GetMany(d => d.Status == "Active");
            return data;
        }
    }

}
