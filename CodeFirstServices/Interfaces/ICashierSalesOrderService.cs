using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierSalesOrderService
    {
        void Create(CashierSalesOrder cash);
        void Update(CashierSalesOrder cash);
        IEnumerable<GetCashRowsBySO> GetRowsByOrderNo(string procname);
        IEnumerable<CashierSalesOrder> GetOrderNoByDate(DateTime date);
        IEnumerable<CashierSalesOrder> GetRowsByDate(string orderno, DateTime date);
        CashierSalesOrder GetDetailsBySONo(string orderno);
        CashierSalesOrder GetDetailsByCashierCode(string code);
        IEnumerable<CashierSalesOrder> GetSalesOrderNos(string order);
        IEnumerable<CashierSalesOrder> GetRowsBySONo(string orderno);
        CashierSalesOrder GetDetailsBySONoAndStatus(string orderno);
        IEnumerable<CashierSalesOrder> GetBillsByHandoverStatus();
        CashierSalesOrder GetRowsBySOAndCashierNo(string orderno, string cashierno);
        IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCard(DateTime date);
        IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCash(DateTime date);
        IEnumerable<CashierSalesOrder> GetOrderNoByDateAndCheque(DateTime date);
        IEnumerable<CashierSalesOrder> GetAll();
        IEnumerable<CashierSalesOrder> GetDailyCashierSalesOrder();
        IEnumerable<CashierSalesOrder> GetActivecashierSalesorder(string orderno);
        IEnumerable<CashierSalesOrder> GetHandoverCashiers();
    }
}
