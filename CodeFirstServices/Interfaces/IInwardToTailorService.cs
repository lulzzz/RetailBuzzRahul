using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardToTailorService
    {
        void Create(InwardToTailor inward);
        InwardToTailor GetLastRowrByFinYr(string year);
        IEnumerable<InwardToTailor> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate);
        IEnumerable<InwardToTailor> GetActiveTailor();
    }
}
