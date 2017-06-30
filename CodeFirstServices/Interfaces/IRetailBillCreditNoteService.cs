using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IRetailBillCreditNoteService
    {
        void Create(RetailBillCreditNote CreditNote);
        void Update(RetailBillCreditNote CreditNote);
        RetailBillCreditNote GetLastCreditNoteByFinYr(string year, string shopcode);
        IEnumerable<RetailBillCreditNote> GetRetailBillCreditNoteNosFromClient(string name, string client);
        IEnumerable<RetailBillCreditNote> GetSalesBillCreditNoteNosFromClient(string name, string client);
        RetailBillCreditNote GetCreditNoteDetails(string no);
        IEnumerable<RetailBillCreditNote> GetCreditNoteFromRetailBill(string name, string billno);
        IEnumerable<RetailBillCreditNote> GetActiveCreditNoteDetails();
        IEnumerable<RetailBillCreditNote> GetAllCreditNotes();
        IEnumerable<RetailBillCreditNote> GetCreditNotesByClient(string clientname);
        IEnumerable<RetailBillCreditNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate);
        IEnumerable<RetailBillCreditNote> GetRowsByFromAndToDateAndClient(DateTime fromdate, DateTime todate, string client);
        IEnumerable<RetailBillCreditNote> GetBillsByDate(DateTime date);
    }
}
