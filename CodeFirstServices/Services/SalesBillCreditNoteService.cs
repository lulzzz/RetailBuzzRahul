using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class SalesBillCreditNoteService : ISalesBillCreditNoteService
    {
        private readonly ISalesBillCreditNoteRepository _SalesBillCreditNoteRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public SalesBillCreditNoteService(ISalesBillCreditNoteRepository SalesBillCreditNoteRepository, IUnitOfWork UnitOfWork)
        {
            this._SalesBillCreditNoteRepository = SalesBillCreditNoteRepository;
            this._UnitOfWork = UnitOfWork;
        }
        public void Create(SalesBillCreditNote SalesBillCreditNote)
        {
            _SalesBillCreditNoteRepository.Add(SalesBillCreditNote);
            _UnitOfWork.Commit();
        }

        public void Update(SalesBillCreditNote SalesBillCreditNote)
        {
            _SalesBillCreditNoteRepository.Update(SalesBillCreditNote);
            _UnitOfWork.Commit();
        }

        public SalesBillCreditNote GetCreditNoteDetails(string no)
        {
            var data = _SalesBillCreditNoteRepository.Get(c => c.CreditNoteNo == no);
            return data;
        }

        public SalesBillCreditNote GetLastCreditNoteByFinYr(string year, string shopcode)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(c => c.CreditNoteNo.Contains(year) && c.ShopCode == shopcode).OrderBy(c => c.CreditNoteNo).LastOrDefault();
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetCreditNoteFromSalesBill(string name, string billno)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(c => c.CreditNoteNo.ToLower().Contains(name.ToString().ToLower()) && c.BillNo == billno && c.Status == "Active").OrderBy(c => c.CreditNoteNo);
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetSalesBillCreditNoteNosFromClient(string name, string client)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(c => c.CreditNoteNo.ToLower().Contains(name.ToString().ToLower()) && c.ClientName == client && c.BillType == "Sales Bill" && c.Status == "Active").OrderBy(c => c.CreditNoteNo);
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetActiveSalesBillCreditNoteDetails()
        {
            var data = _SalesBillCreditNoteRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetAllCreditNotes()
        {
            var data = _SalesBillCreditNoteRepository.GetAll();
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetCreditNotesByClient(string clientname)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(c => c.ClientName == clientname);
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(d => Convert.ToDateTime(d.Date).Date >= fromdate.Date && Convert.ToDateTime(d.Date).Date <= todate.Date);
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetRowsByFromAndToDateAndClient(DateTime fromdate, DateTime todate, string client)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(d => Convert.ToDateTime(d.Date).Date >= fromdate.Date && Convert.ToDateTime(d.Date).Date <= todate.Date && d.ClientName == client);
            return data;
        }

        public IEnumerable<SalesBillCreditNote> GetBillsByDate(DateTime date)
        {
            var data = _SalesBillCreditNoteRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }
    }
}
