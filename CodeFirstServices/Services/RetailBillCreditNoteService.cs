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
    public class RetailBillCreditNoteService : IRetailBillCreditNoteService
    {
        private readonly IRetailBillCreditNoteRepository _CreditNoteRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public RetailBillCreditNoteService(IRetailBillCreditNoteRepository CreditNoteRepository, IUnitOfWork UnitOfWork)
        {
            this._CreditNoteRepository = CreditNoteRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(RetailBillCreditNote CreditNote)
        {
            _CreditNoteRepository.Add(CreditNote);
            _UnitOfWork.Commit();
        }

        public void Update(RetailBillCreditNote CreditNote)
        {
            _CreditNoteRepository.Update(CreditNote);
            _UnitOfWork.Commit();
        }

        public IEnumerable<RetailBillCreditNote> GetRetailBillCreditNoteNosFromClient(string name, string client)
        {
            var data = _CreditNoteRepository.GetMany(c => c.CreditNoteNo.ToLower().Contains(name.ToString().ToLower()) && c.ClientName == client && c.BillType == "Retail Bill" && c.Status == "Active").OrderBy(c => c.CreditNoteNo);
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetSalesBillCreditNoteNosFromClient(string name, string client)
        {
            var data = _CreditNoteRepository.GetMany(c => c.CreditNoteNo.ToLower().Contains(name.ToString().ToLower()) && c.ClientName == client && c.BillType == "Sales Bill").OrderBy(c => c.CreditNoteNo);
            return data;
        }

        public RetailBillCreditNote GetCreditNoteDetails(string no)
        {
            var data = _CreditNoteRepository.Get(d => d.CreditNoteNo == no);
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetCreditNoteFromRetailBill(string name, string billno)
        {
            var data = _CreditNoteRepository.GetMany(c => c.CreditNoteNo.ToLower().Contains(name.ToString().ToLower()) && c.BillNo == billno && c.Status == "Active").OrderBy(c => c.CreditNoteNo);
            return data;
        }

        public RetailBillCreditNote GetLastCreditNoteByFinYr(string year, string shopcode)
        {
            var data = _CreditNoteRepository.GetMany(c => c.CreditNoteNo.Contains(year) && c.ShopCode == shopcode).OrderBy(c => c.CreditNoteNo).LastOrDefault();
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetActiveCreditNoteDetails()
        {
            var data = _CreditNoteRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetAllCreditNotes()
        {
            var data = _CreditNoteRepository.GetAll();
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetCreditNotesByClient(string clientname)
        {
            var data = _CreditNoteRepository.GetMany(c => c.ClientName == clientname);
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var data = _CreditNoteRepository.GetMany(d => Convert.ToDateTime(d.Date).Date >= fromdate.Date && Convert.ToDateTime(d.Date).Date <= todate.Date);
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetRowsByFromAndToDateAndClient(DateTime fromdate, DateTime todate,string client)
        {
            var data = _CreditNoteRepository.GetMany(d => Convert.ToDateTime(d.Date).Date >= fromdate.Date && Convert.ToDateTime(d.Date).Date <= todate.Date && d.ClientName == client);
            return data;
        }

        public IEnumerable<RetailBillCreditNote> GetBillsByDate(DateTime date)
        {
            var data = _CreditNoteRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }
    }
}
