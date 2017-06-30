using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IStockDistributionService
    {
        void Create(StockDistribution stock);
        StockDistribution GetLastRow();
        StockDistribution GetDetailsByCode(string Code);
        IEnumerable<StockDistribution> GetDetailsByGodownCode(string godowncode);
        StockDistribution GetDetailsById(int id);
        StockDistribution GetLastStockDisByFinYr(string year);
        IEnumerable<StockDistribution> GetItemsByStatus();
        IEnumerable<StockDistribution> GetStkDisNos(string no);
        StockDistribution GetReferenceNo(string RefNo);
    }
}
