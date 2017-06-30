using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardToClientService
    {
        void Create(OutwardToClient data);
        OutwardToClient GetLastRow();
        OutwardToClient GetLastOutwardByFinYr(string year,string Code);
        IEnumerable<OutwardToClient> GetOutWardToClientByDate();
        IEnumerable<OutwardToClient> GetDetailsByDate(DateTime fromdate, DateTime todate);
        IEnumerable<OutwardToClient> GetReportByClientAndDate(string Client, DateTime fromdate, DateTime todate);
        OutwardToClient GetDetailsById(int id);
        IEnumerable<OutwardToClient> GetOutwardNo(string term, string code);
        IEnumerable<OutwardToClient> GetOutwardNoForGodown(string term, string code);
        OutwardToClient GetDetailsByOutwardCode(string OutwardNo);
    }
}
