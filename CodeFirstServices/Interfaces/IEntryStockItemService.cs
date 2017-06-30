using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IEntryStockItemService
    {
        void Create(EntryStockItem stock);
        void Update(EntryStockItem stock);
        IEnumerable<EntryStockItem> GetAllItems();
        IEnumerable<EntryStockItem> GetEntryStockItems(string items);
        EntryStockItem getDetailsByItemName(string itemname);
        EntryStockItem getDetailsByItemCode(string itemcode);
        IEnumerable<EntryStockItem> GetListByItemCode(string code);
        IEnumerable<EntryStockItem> GetDetailsByDate(DateTime fromdate, DateTime todate);
        IEnumerable<EntryStockItem> GetDetailsByCategory(string Category);
        IEnumerable<EntryStockItem> GetItemsByQuantity();
        IEnumerable<EntryStockItem> GetDetailsBySubCategory(string SubCat);
        IEnumerable<EntryStockItem> GetDetailsByEntryStockCode(string no);
        EntryStockItem getDetailsByItemNameandItemCodeAndBrandName(string itemname, string itemcode, string brandname);
    }
}
