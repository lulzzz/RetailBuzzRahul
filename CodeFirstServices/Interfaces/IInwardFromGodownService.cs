using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CodeFirstServices.Interfaces
{
   public interface IInwardFromGodownService
    {
       void Create(InwardFromGodown InwardFromGodown);
       InwardFromGodown GetLastRow();
       IEnumerable<InwardFromGodown> GetAll();
       InwardFromGodown GetDetailsByCode(string code);
       InwardFromGodown GetDetailsById(int id);
       IEnumerable<InwardFromGodown> GetReportByDate(DateTime fromdate, DateTime todate);
       IEnumerable<InwardFromGodown> GetReportByGodownNameAndDate(string Godown, DateTime fromdate, DateTime todate);
       IEnumerable<InwardFromGodown> GetInwardNo(string term, string shopcode);
       IEnumerable<InwardFromGodown> GetDailyInwardsFromGodown();
       InwardFromGodown GetLastInwardByFinYear(string Year,string ShopCode);
    }
}
