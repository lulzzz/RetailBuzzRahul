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
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _QuotationRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public QuotationService(IQuotationRepository QuotationRepository, IUnitOfWork UnitOfWork)
        {
            this._QuotationRepository = QuotationRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public Quotation GetLastQuotByFinYr(string year, string shopcode)
        {
            var data = _QuotationRepository.GetMany(q => q.QuotNo.Contains(year) && q.ShopCode == shopcode).OrderBy(q => q.QuotNo).LastOrDefault();
            return data;
        }

        public void Create(Quotation Quotation)
        {
            _QuotationRepository.Add(Quotation);
            _UnitOfWork.Commit();
        }

        public void Update(Quotation Quotation)
        {
            _QuotationRepository.Update(Quotation);
            _UnitOfWork.Commit();
        }

        public IEnumerable<Quotation> GetQuotations(string clientname, string quotno)
        {
            var data = _QuotationRepository.GetMany(q => q.ClientName == clientname && q.QuotNo.ToLower().StartsWith(quotno.ToString().ToLower())).OrderBy(q => q.QuotNo);
            return data;
        }

        public IEnumerable<Quotation> GetActiveQuotNo(string quotno)
        {
            var data = _QuotationRepository.GetMany(q => q.Status == "Active" && q.SendingStatus != "Completed" && (q.ProcessedIn.Contains("SO") || q.ProcessedIn == "Fresh") && q.QuotNo.ToLower().Contains(quotno.ToString().ToLower())).OrderBy(q => q.QuotNo);
            return data;
        }

        public IEnumerable<Quotation> GetQuotNo(string quotno)
        {
            var data = _QuotationRepository.GetMany(q => q.QuotNo.ToLower().Contains(quotno.ToString().ToLower()) && q.Status == "Active" && q.ProcessedIn == "Fresh");
            return data;
        }

        public Quotation GetQuotByCode(string quotno)
        {
            var data = _QuotationRepository.Get(q => q.QuotNo == quotno);
            return data;
        }

        public Quotation GetQuotById(int id)
        {
            var details = _QuotationRepository.GetById(id);
            return details;
        }

        public IEnumerable<Quotation> GetAll()
        {
            var data = _QuotationRepository.GetAll();
            return data;
        }

        public IEnumerable<Quotation> GetQuotByDate(DateTime fromdate, DateTime todate)
        {
            var details = _QuotationRepository.GetMany(q => Convert.ToDateTime(q.QuotDate).Date >= fromdate.Date && Convert.ToDateTime(q.QuotDate).Date <= todate.Date && q.SendingStatus != "Completed" && q.Status == "Active");
            return details;
        }

        public IEnumerable<Quotation> GetOnlyPendingQuotations()
        {
            var quotlist = _QuotationRepository.GetMany(q => q.Status == "Active" && q.ProcessedIn == "Fresh" && q.SendingStatus == "Pending");
            return quotlist;
        }

        public IEnumerable<Quotation> GetQuotProcessedInChallanAndSales(string quotno)
        {
            var list = _QuotationRepository.GetMany(q => q.Status == "Active" && q.SendingStatus != "Completed" && (q.ProcessedIn == "Fresh" || q.ProcessedIn.Contains("SO") || q.ProcessedIn.Contains("DC")) && q.QuotNo.ToLower().Contains(quotno.ToString().ToLower())).OrderBy(q => q.QuotNo);
            return list;
        }

        public IEnumerable<Quotation> GetClientQuotList(string clientname)
        {
            var list = _QuotationRepository.GetMany(q => q.ClientName == clientname && q.Status == "Active" && (q.ProcessedIn == "Fresh" || q.ProcessedIn.Contains("SO") || q.ProcessedIn.Contains("DC")) && q.SendingStatus != "Completed");
            return list;
        }

        public IEnumerable<Quotation> GetActiveQuotationNo(string quotno)
        {
            var data = _QuotationRepository.GetMany(q => q.Status == "Active" && q.QuotNo.ToLower().StartsWith(quotno.ToString().ToLower())).OrderBy(q => q.QuotNo);
            return data;
        }

        public Quotation GetDetailsByQuotNo(string quotno)
        {
            var data = _QuotationRepository.Get(d => d.QuotNo == quotno);
            return data;
        }

        public IEnumerable<Quotation> GetDetailsByClientName(string client)
        {
            var details = _QuotationRepository.GetMany(d => d.ClientName == client && d.Status == "Active");
            return details;
        }

        public IEnumerable<Quotation> GetQuotListByClientNameAndProcessedIn(string clientname)
        {
            var list = _QuotationRepository.GetMany(q => q.ClientName == clientname && q.ProcessedIn != "Fresh");
            return list;
        }

        public IEnumerable<Quotation> GetDailyQuotations()
        {
            var list = _QuotationRepository.GetMany(q => Convert.ToDateTime(q.QuotDate).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<Quotation> GetPendingQuotationsPoForCarryForward()
        {
            var data = _QuotationRepository.GetMany(p => p.SendingStatus != "Completed" && p.Status == "Active");
            return data;
        }

        public IEnumerable<Quotation> GetAllQuotations(string term)
        {
            var list = _QuotationRepository.GetMany(q => q.QuotNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(q => q.QuotNo);
            return list;
        }

        public IEnumerable<Quotation> GetActiveQuotations()
        {
            var data = _QuotationRepository.GetMany(d => d.Status == "Active");
            return data;
        }
    }
}
