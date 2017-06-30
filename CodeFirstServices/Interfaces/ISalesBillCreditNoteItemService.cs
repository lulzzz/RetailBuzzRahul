using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesBillCreditNoteItemService
    {
        void Create(SalesBillCreditNoteItem SalesBillCreditNoteItem);
        IEnumerable<SalesBillCreditNoteItem> GetAllItemsByCreditNote(string creditnoteno);
        IEnumerable<SalesBillCreditNoteItem> GetItemListByCreditNote(string creditnoteno);
        IEnumerable<SalesBillCreditNoteItem> GetItemListByCreditNoteandItemType(string creditnoteno);
        IEnumerable<SalesBillCreditNoteItem> GetTaxFreeItemsByDate(DateTime Date);
        IEnumerable<SalesBillCreditNoteItem> GetOnePerTaxItemsByDate(DateTime Date);
        IEnumerable<SalesBillCreditNoteItem> GetFivePointFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<SalesBillCreditNoteItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date);
    }
}
