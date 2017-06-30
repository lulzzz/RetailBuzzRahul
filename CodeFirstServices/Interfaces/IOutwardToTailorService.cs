using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardToTailorService
    {
        void Create(OutwardToTailor outward);
        void Update(OutwardToTailor outward);
        OutwardToTailor GetLastRow();
        OutwardToTailor GetDetailsById(int id);
        OutwardToTailor GetLastRowrByFinYr(string year);
        IEnumerable<OutwardToTailor> GetOutwardNo(string term);
        IEnumerable<OutwardToTailor> GetOutwardNoByStatusAndInwardStatus(string term);
        OutwardToTailor GetDetailsByCode(string code);
        IEnumerable<OutwardToTailor> GetOutwardNoByInwardStatus(string term);
        IEnumerable<OutwardToTailor> GetPendingOutwardToTailors();
        IEnumerable<OutwardToTailor> GetRowsByStatusAndInwardStatus(string term);
        IEnumerable<OutwardToTailor> GetRowsByTailotName(string name);
        IEnumerable<OutwardToTailor> GetOutwardNoByInActiveInwardStatus(string term);
        IEnumerable<OutwardToTailor> GetOutwardNoByInwardAndsTailorInwardStatus(string term);
        IEnumerable<OutwardToTailor> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate);
    }
}
