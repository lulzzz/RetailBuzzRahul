using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesBillItemService
    {
        void Create(SalesBillItem bill);
        void Update(SalesBillItem bill);
        void Delete(SalesBillItem bill);
        SalesBillItem GetItemById(int id);
        IEnumerable<SalesBillItem> GetItemsBySalesBillNo(string bill);
        SalesBillItem GetItemDetailsByinvoiceNoAndItemCode(string code, string itemcode);
        IEnumerable<SalesBillItem> GetItemsByCodeAndType(string no);
        IEnumerable<SalesBillItem> GetTaxFreeItemsByDate(DateTime Date);
        IEnumerable<SalesBillItem> GetOnePerTaxItemsByDate(DateTime Date);
        IEnumerable<SalesBillItem> GetFivePointFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<SalesBillItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<SalesBillItem> GetActiveSalesBillNo(string no);

        IEnumerable<SalesBillItem> GetTaxFreeItemsByBillNo(string billno);
        IEnumerable<SalesBillItem> GetOnePerTaxItemsByBillNo(string billno);
        IEnumerable<SalesBillItem> GetFivePointFivePerTaxItemsByBillNo(string billno);
        IEnumerable<SalesBillItem> GetTwelvePointFivePerTaxItemsByBillNo(string billno);
    }
}
