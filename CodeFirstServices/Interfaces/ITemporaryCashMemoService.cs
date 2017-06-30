using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ITemporaryCashMemoService
    {
        void Create(TemporaryCashMemo cashmemo);
        void Update(TemporaryCashMemo cashmemo);
        TemporaryCashMemo GetById(int id);
        TemporaryCashMemo GetLastInvoiceDetails();
        TemporaryCashMemo GetLastTempCashMemoByFinYr(string year, string shopcode);
        IEnumerable<TemporaryCashMemo> GetAll();
        IEnumerable<TemporaryCashMemo> GetRetailNos(string billno);
        TemporaryCashMemo GetDetailsByInvoiceNo(string no);
        TemporaryCashMemo GetDetailsById(int id);
        IEnumerable<TemporaryCashMemo> GetRetailBillByDate();
        IEnumerable<TemporaryCashMemo> GetRetailByStatus(string retail);
        IEnumerable<TemporaryCashMemo> GetRetailByCashierStatus(string retail);
        IEnumerable<TemporaryCashMemo> GetReportByDate(DateTime FromDate, DateTime ToDate, string Emp);
        TemporaryCashMemo GetSalesPersonByShopName(string ShopName);
        IEnumerable<TemporaryCashMemo> GetBillsByDate(DateTime date);
        IEnumerable<TemporaryCashMemo> GetBillsByDateAndAdjustAmount(DateTime date);
        IEnumerable<TemporaryCashMemo> GetBillsByClientName(string client);
        IEnumerable<TemporaryCashMemo> GetBillsByConvertStatus();
        IEnumerable<TemporaryCashMemo> GetDetailsByCashierAndConvertStatus(string retail);
        IEnumerable<TemporaryCashMemo> GetBillsByDateAndConvertStatus(DateTime date);
        IEnumerable<TemporaryCashMemo> GetAllActiveTemporaryCashMemo();
    }
}
