using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IStockItemDistributionService
    {
        void Create(StockItemDistribution stock);
        void Update(StockItemDistribution stock);
        IEnumerable<StockItemDistribution> GetRowsByItemCode(string code);
        StockItemDistribution GetDetailsByItemCode(string code);
        StockItemDistribution GetLastRowFromItemAndGodownCode(string itemcode, string godowncode);
        StockItemDistribution GetDetailsByCode(string Code);
        IEnumerable<StockItemDistribution> GetDetailsByGodownCode(string code);
        IEnumerable<StockItemDistribution> GetRowsByStockDistributionCode(string code);
        IEnumerable<ListOfItemCode> GetItemsUsingStoreProc(string procname, object[] GodownCode);
        StockItemDistribution GetDetailsByItemCodeAndGodownCode(string itemcode, string godowncode);
        StockItemDistribution GetDetailsByItemCodeAndShopCode(string itemcode, string shopcode);
        IEnumerable<GetAllItemsFromStkDistrbtn> GetItemsBySubcategoryUsingProc(string procname, object[] Subcat);
        IEnumerable<StockItemDistribution> GetDetailsByStkCode(string Code);
    }
}
