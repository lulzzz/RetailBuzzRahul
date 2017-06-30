using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesReturnService
    {
        void Create(SalesReturn SalesReturn);
        void Update(SalesReturn SalesReturn);
        IEnumerable<SalesReturn> GetAllSalesReturnNo(string srno);
        SalesReturn GetLastRow();
        SalesReturn GetLastSalesReturnByFinYr(string year, string shopcode);
        SalesReturn GetLastSalesOfBillNo(string billno);
        SalesReturn GetById(int id);
        IEnumerable<SalesReturn> GetAllSalesOfBill(string billno);
        SalesReturn GetDataByBillNoAndStatus(string billno);
        SalesReturn GetSalesByReturnNo(string srno);
        IEnumerable<SalesReturn> GetDailySalesReturns();
        IEnumerable<SalesReturn> GetReturnsByBillNo(string no);
        IEnumerable<SalesReturn> GetBillsByDate(DateTime date);
    }
}
