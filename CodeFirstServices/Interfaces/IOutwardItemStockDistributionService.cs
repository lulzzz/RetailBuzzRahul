using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardItemStockDistributionService
    {
        void Create(OutwardItemStockDistribution data);
        OutwardItemStockDistribution GetLastRowFromItemAndGodownCode(string itemcode, string godowncode);
        IEnumerable<OutwardItemStockDistribution> GetItemsByOutwardCode(string code);
        void Update(OutwardItemStockDistribution data);
        OutwardItemStockDistribution GetItemDetailsByItemCode(string itemcode);
        IEnumerable<OutwardItemStockDistribution> GetItemsById(int id);
        OutwardItemStockDistribution GetItemByItemAndOutwardCode(string outwardcode, string itemcode);
    }
}
