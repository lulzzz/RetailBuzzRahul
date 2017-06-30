using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IQuotationItemService
    {
        IEnumerable<QuotationItem> GetDetailsByQuotNo(string quotno);
        QuotationItem GetRowByQuotNo(string quotno);
        void Create(QuotationItem QuotationItem);
        void Update(QuotationItem QuotationItem);
        void Delete(QuotationItem QuotationItem);
        IEnumerable<QuotationItem> GetItemsByCode(string code);
        QuotationItem GetDetailsByItemCode(string itemcode);
        IEnumerable<QuotationItem> GetRowsByQuotNoAndStatus(string quotno);
        QuotationItem GetItemDetailsByItemCodeAndQuotNo(string itemcode, string quotno);
        IEnumerable<QuotationItem> GetInventoryItemsByQuotNo(string quotno);
        IEnumerable<QuotationItem> GetAllActiveItemsByQuotNo(string quotno);
    }
}
