using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesOrderService
    {
        SalesOrder GetLastRow();
        SalesOrder GetLastSalerOrderByFinYr(string year, string shopcode);
        void Create(SalesOrder data);
        void Update(SalesOrder OrderProcessing);
        IEnumerable<SalesOrder> GetOrderNos(string order);
        SalesOrder GetOrderDetails(string code);
        SalesOrder GetDetailsBySalesOrderNo(string no);
        IEnumerable<SalesOrder> GetOrdersByStatus(string orderno);
        IEnumerable<SalesOrder> GetAll();
        IEnumerable<SalesOrder> GetAllSalesOrders(string term);
        SalesOrder GetDetailsById(int id);
        IEnumerable<SalesOrder> GetAllActiveSalesOrder();
        SalesOrder GetDetailsByOrderNo(string orderno);
        IEnumerable<SalesOrder> GetDetailsByQuotNo(string quotno);
        IEnumerable<SalesOrder> GetDetailsByStatusAndCashierStatus(string term);
        IEnumerable<SalesOrder> GetDetailsByClientName(string client);
        IEnumerable<SalesOrder> GetDetailsByClientAndBalance(string client);
        IEnumerable<SalesOrder> GetActiveSalesOrderForAutoComplete(string orderno);
        IEnumerable<SalesOrder> GetDailySalesOrders();

    }
}
