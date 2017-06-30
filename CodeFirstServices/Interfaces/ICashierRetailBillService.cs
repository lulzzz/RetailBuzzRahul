using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierRetailBillService
    {
        void Create(CashierRetailBill cash);
        void Update(CashierRetailBill cash);
        CashierRetailBill GetDetailsByRetailNo(string retailno);
        IEnumerable<CashierRetailBill> GetRowsByRBNo(string billno);
        IEnumerable<CashierRetailBill> GetBillsByHandoverStatus();
        CashierRetailBill GetRowsByRIAndCashierNo(string retailno, string cashierno);
        IEnumerable<CashierRetailBill> GetBillsByDate(DateTime date);
        IEnumerable<CashierRetailBill> GetBillsByDateAndDateStatus(DateTime date);
        IEnumerable<CashierRetailBill> GetBillsByDateAndRetailBillNo(DateTime date, string billno);
        CashierRetailBill GetDataByStatusAndRetailBillNo(string billno);
        IEnumerable<CashierRetailBill> GetAll();
        IEnumerable<CashierRetailBill> GetDailyCashierRetailBill();
        IEnumerable<CashierRetailBill> GetActivecashierRetailbill(string orderno);
        IEnumerable<CashierRetailBill> GetHandoverRetailCashier();
        IEnumerable<CashierRetailBill> GetRowsByDate(string retailbillno, DateTime date);
    }
}
