using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IRetailBillCreditNoteItemService
    {
        void Create(RetailBillCreditNoteItem CreditNoteItem);
        IEnumerable<RetailBillCreditNoteItem> GetAllItemsByCreditNote(string creditnoteno);
        IEnumerable<RetailBillCreditNoteItem> GetItemListByCreditNote(string creditnoteno);
        IEnumerable<RetailBillCreditNoteItem> GetItemListByCreditNoteandItemType(string creditnoteno);
        IEnumerable<RetailBillCreditNoteItem> GetTaxFreeItemsByDate(DateTime Date);
        IEnumerable<RetailBillCreditNoteItem> GetOnePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillCreditNoteItem> GetFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillCreditNoteItem> GetFivePointFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillCreditNoteItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date);
    }
}
