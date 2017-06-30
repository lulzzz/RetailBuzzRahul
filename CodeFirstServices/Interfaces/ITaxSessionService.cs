using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeFirstServices.Interfaces
{
    public interface ITaxSessionService
    {
        void Create(TaxSessionMaster tax);
        TaxSessionMaster GetLastRow();
        TaxSessionMaster GetDetailsById(int id);
        TaxSessionMaster GetDetailsByTaxCode(string code);
        IEnumerable<TaxSessionMaster> GetDetailsByDate(DateTime fromdate, DateTime todate);
    }
}
