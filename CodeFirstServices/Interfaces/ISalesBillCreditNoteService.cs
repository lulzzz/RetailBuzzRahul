using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesBillCreditNoteService
    {
        void Create(SalesBillCreditNote SalesBillCreditNote);
        void Update(SalesBillCreditNote SalesBillCreditNote);
        SalesBillCreditNote GetCreditNoteDetails(string no);
        SalesBillCreditNote GetLastCreditNoteByFinYr(string year, string shopcode);
        IEnumerable<SalesBillCreditNote> GetCreditNoteFromSalesBill(string name, string billno);
        IEnumerable<SalesBillCreditNote> GetSalesBillCreditNoteNosFromClient(string name, string client);
        IEnumerable<SalesBillCreditNote> GetActiveSalesBillCreditNoteDetails();
        IEnumerable<SalesBillCreditNote> GetAllCreditNotes();
        IEnumerable<SalesBillCreditNote> GetCreditNotesByClient(string clientname);
        IEnumerable<SalesBillCreditNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate);
        IEnumerable<SalesBillCreditNote> GetRowsByFromAndToDateAndClient(DateTime fromdate, DateTime todate, string client);
        IEnumerable<SalesBillCreditNote> GetBillsByDate(DateTime date);
    }
}
