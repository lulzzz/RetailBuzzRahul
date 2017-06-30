using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierPayableService
    {
        void Create(CashierPayable cash);
        CashierPayable GetLastRow();
        CashierPayable GetLastCashierByFinYr(string year);
    }
}
