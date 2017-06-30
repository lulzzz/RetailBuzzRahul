using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierSalesBillService
    {
        void Create(CashierSalesBill cash);
        void Update(CashierSalesBill cash);
        IEnumerable<CashierSalesBill> GetRowsBySBNo(string billno);
        IEnumerable<CashierSalesBill> GetBillsByHandoverStatus();
        CashierSalesBill GetDetailsByCashierCode(string cashcode);
        CashierSalesBill GetRowsBySBAndCashierNo(string salesno, string cashierno);
        IEnumerable<CashierSalesBill> GetBillsByDate(DateTime date);
        CashierSalesBill GetDetailsBySBNo(string salesno);
        IEnumerable<CashierSalesBill> GetAll();
        IEnumerable<CashierSalesBill> GetDailyCashierSalesBill();
        IEnumerable<CashierSalesBill> GetActivecashierSalesbill(string salesbill);
        IEnumerable<CashierSalesBill> GetHandoverSalesBillCashier();
        IEnumerable<CashierSalesBill> GetRowsByDate(string salesbillno, DateTime date);
        CashierSalesBill GetDataByStatusAndSalesBillNo(string billno);
    }
}
