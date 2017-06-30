using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardInterGodownService
    {
        OutwardInterGodown GetLastRow();
        OutwardInterGodown GetLastOutwardByFinYr(string year,string GdCode);
        void Create(OutwardInterGodown outward);
        void Update(OutwardInterGodown outward);
        OutwardInterGodown GetDetailsByOutwardId(int id);
        OutwardInterGodown GetDetailsByOutwardCode(string OutwardCode);
        IEnumerable<OutwardInterGodown> GetReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<OutwardInterGodown> GetOutwardNos(string term, string GodownCode);
        IEnumerable<OutwardInterGodown> GetOutwardNoForPrint(string term, string GodownCode);
        IEnumerable<OutwardInterGodown> GetDailyOutwardInterGodown();
        void Delete(OutwardInterGodown outward);
    }
}
