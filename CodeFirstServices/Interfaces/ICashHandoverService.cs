using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashHandoverService
    {
        CashHandover GetLastRow();
        void Create(CashHandover cash);
        IEnumerable<CashHandover> GetDataByDate(DateTime date);
        CashHandover GetLastRowByFinYr(string year);
        IEnumerable<CashHandover> GetAll();
        IEnumerable<CashHandover> GetDailyCashHandover();
    }
}
