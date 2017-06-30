using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IRetailBillService
    {
        void Create(RetailBill retailinvoicemaster);
        void Update(RetailBill retailinvoicemaster);
        RetailBill GetById(int id);
        RetailBill GetLastInvoiceDetails();
        RetailBill GetLastRetailBillByFinYr(string year, string shopcode);
        IEnumerable<RetailBill> GetAll();
        IEnumerable<RetailBill> GetRetailNos(string billno);
        RetailBill GetDetailsByInvoiceNo(string no);
        RetailBill GetDetailsById(int id);
        IEnumerable<RetailBill> GetRetailBillByDate();
        IEnumerable<RetailBill> GetRetailByStatus(string retail);
        IEnumerable<RetailBill> GetRetailByCashierStatus(string retail);
        //IEnumerable<RetailBill> GetRetailNoByStatus(string retail);
        IEnumerable<RetailBill> GetReportByDate(DateTime FromDate, DateTime ToDate, string Emp);
        RetailBill GetSalesPersonByShopName(string ShopName);
        IEnumerable<RetailBill> GetBillsByDate(DateTime date);
        IEnumerable<RetailBill> GetBillsByDateAndAdjustAmount(DateTime date);
        IEnumerable<RetailBill> GetBillsByClientName(string client);
        IEnumerable<RetailBill> GetAllRetailBillNos(string term);
        IEnumerable<RetailBill> GetRetailBillByFromAndToDate(DateTime fromdate, DateTime todate);
        IEnumerable<RetailBill> GetRetailBillNosByWithTaxStatus(string term, string taxstatus);
        IEnumerable<RetailBill> GetRetailBillNosByWithoutTaxStatus(string term, string taxstatus);
        IEnumerable<RetailBill> GetAllActiveRetailBill();
    }
}
