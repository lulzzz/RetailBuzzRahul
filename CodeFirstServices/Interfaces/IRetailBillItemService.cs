using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IRetailBillItemService
    {
        void Create(RetailBillItem retailinvoiceitem);
        void Update(RetailBillItem retailinvoiceitem);
        void Delete(RetailBillItem retailinvoiceitem);
        RetailBillItem GetById(int id);
        RetailBillItem GetLastItem();
        IEnumerable<RetailBillItem> GetAll();
        IEnumerable<RetailBillItem> GetDetailsByInvoiceNo(string no);
        IEnumerable<RetailBillItem> GetDetailsByRetailBillNo(string code);
        RetailBillItem GetItemDetailsByBillNoAndItemCode(string code, string itemcode);
        IEnumerable<RetailBillItem> GetDetailsByInvoiceNoAndStatus(string no);
        IEnumerable<RetailBillItem> GetItemsByCodeAndType(string no);
        IEnumerable<RetailBillItem> GetTaxFreeItemsByDate(DateTime Date);
        IEnumerable<RetailBillItem> GetOnePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillItem> GetFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillItem> GetFivePointFivePerTaxItemsByDate(DateTime Date);
        IEnumerable<RetailBillItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date);

        IEnumerable<RetailBillItem> GetTaxFreeItemsByBillNo(string billno);
        IEnumerable<RetailBillItem> GetOnePerTaxItemsByBillNo(string billno);
        IEnumerable<RetailBillItem> GetFivePerTaxItemsByBillNo(string billno);
        IEnumerable<RetailBillItem> GetFivePointFivePerTaxItemsByBillNo(string billno);
        IEnumerable<RetailBillItem> GetTwelvePointFivePerTaxItemsByBillNo(string billno);
    }
}
