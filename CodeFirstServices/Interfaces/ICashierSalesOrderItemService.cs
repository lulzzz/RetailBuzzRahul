using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierSalesOrderItemService
    {
        void Create(CashierSalesOrderItem cash);
        IEnumerable<CashierSalesOrderItem> GetDetailsByCashierCode(string code);
        IEnumerable<CashierSalesOrderItem> GetRowsBySONo(string orderno);
    }
}
