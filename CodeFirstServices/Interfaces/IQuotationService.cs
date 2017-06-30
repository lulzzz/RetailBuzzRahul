using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IQuotationService
    {
        Quotation GetLastQuotByFinYr(string year, string shopcode);
        void Create(Quotation Quotation);
        void Update(Quotation Quotation);
        IEnumerable<Quotation> GetQuotations(string clientname, string quotno);
        IEnumerable<Quotation> GetActiveQuotNo(string quotno);
        IEnumerable<Quotation> GetQuotProcessedInChallanAndSales(string quotno);
        Quotation GetQuotByCode(string quotno);
        Quotation GetQuotById(int id);
        IEnumerable<Quotation> GetAll();
        IEnumerable<Quotation> GetAllQuotations(string term);
        IEnumerable<Quotation> GetQuotByDate(DateTime fromdate, DateTime todate);
        IEnumerable<Quotation> GetQuotNo(string quotno);
        IEnumerable<Quotation> GetOnlyPendingQuotations();
        IEnumerable<Quotation> GetClientQuotList(string clientname);
        IEnumerable<Quotation> GetActiveQuotationNo(string quotno);
        Quotation GetDetailsByQuotNo(string quotno);
        IEnumerable<Quotation> GetDetailsByClientName(string client);
        IEnumerable<Quotation> GetQuotListByClientNameAndProcessedIn(string clientname);
        IEnumerable<Quotation> GetDailyQuotations();
        IEnumerable<Quotation> GetPendingQuotationsPoForCarryForward();
        IEnumerable<Quotation> GetActiveQuotations();
    }
}
