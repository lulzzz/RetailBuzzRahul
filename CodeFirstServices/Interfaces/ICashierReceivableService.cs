using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierReceivableService
    {
        void Create(CashierReceivable cash);
        CashierReceivable GetLastRow();
        IEnumerable<CashierReceivable> GetDetailsByDate();
        IEnumerable<CashierReceivable> GetAll();
        CashierReceivable GetDetailsById(int id);
        IEnumerable<CashierReceivable> GetDetailsByBillNo(string billno);
        CashierReceivable GetLastCashierByFinYr(string year);
    }
}
