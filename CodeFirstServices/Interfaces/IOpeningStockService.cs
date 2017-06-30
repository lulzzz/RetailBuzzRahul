using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public   interface IOpeningStockService
    {
        void CreateStock(OpeningStockMaster stock);
        void UpdateStock(OpeningStockMaster stock);
        OpeningStockMaster GetLastRow();
        OpeningStockMaster GetById(int Id);
        OpeningStockMaster GetTotalQuantityByItem(string code);
        IEnumerable<OpeningStockMaster> GetListByStockCode(string stockcode);
        OpeningStockMaster GetDataByItemCode(string code);
        OpeningStockMaster GetDetailsByItemCode(string code);
        IEnumerable<OpeningStockMaster> GetDetailsByDateAndCategory(DateTime fromdate, DateTime todate, string category);
        IEnumerable<OpeningStockMaster> GetDetailsBySubCategory(string subcat);
        IEnumerable<OpeningStockMaster> GetAllItems();
        IEnumerable<OpeningStockMaster> GetItemsByStatus();
        OpeningStockMaster GetDetailsByStockCode(string code);
        OpeningStockMaster GetAllItemsByItemCode(string code);
        OpeningStockMaster GetLastStockByFinYr(string year);
    }
}
