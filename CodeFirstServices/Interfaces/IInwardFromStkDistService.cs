using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardFromStkDistService
    {
        void Create(InwardFromStockDistribution item);
        InwardFromStockDistribution GetLastRow();
        IEnumerable<InwardFromStockDistribution> GetReportByDate(DateTime fromdate, DateTime todate);
        InwardFromStockDistribution GetDetailsById(int id);
        IEnumerable<InwardFromStockDistribution> GetInwardNo(string term, string code);
        InwardFromStockDistribution GetDetailsByInwardNo(string InwardNo);
        InwardFromStockDistribution GetLastInwardByFinYearForShop(string Year,string ShopCode);
        InwardFromStockDistribution GetLastInwardByFinYearForGodown(string Year, string GdCode);
    }
}
