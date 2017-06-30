using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IBalanceCarryForwardService
    {
        void Create(BalanceCarryForward data);
        BalanceCarryForward GetLastRow();
        BalanceCarryForward GetDataByDate(DateTime date);
        BalanceCarryForward GetLastRowByFinYr(string year);
        BalanceCarryForward GetPendingBalances();
    }
}
