using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardInterGodownService
    {
        InwardInterGodown GetLastInward();
        void Create(InwardInterGodown inward);
        InwardInterGodown GetDetailsById(int Id);
        IEnumerable<InwardInterGodown> GetInwardNo(string term, string GdCode);
        InwardInterGodown GetDetailsByInwardCode(string InwardCode);
        IEnumerable<InwardInterGodown> GetDetailsByDate(DateTime FromDate, DateTime ToDate);
        IEnumerable<InwardInterGodown> GetDailyInwardInterGodown();
        InwardInterGodown GetLastInwardByFinYear(string Year,string GdCode);
    }
}
