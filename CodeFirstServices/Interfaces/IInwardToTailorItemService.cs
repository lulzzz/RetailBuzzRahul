using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardToTailorItemService
    {
        void Create(InwardToTailorItem inward);
        IEnumerable<InwardToTailorItem> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate);
        IEnumerable<InwardToTailorItem> GetActiveTailorItemsByCode(string code);
    }
}
