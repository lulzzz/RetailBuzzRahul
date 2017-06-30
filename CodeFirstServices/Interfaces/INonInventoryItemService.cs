using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface INonInventoryItemService
    {
        void createNonInventoryItem(NonInventoryItem item);
        NonInventoryItem GetLastItem();
        IEnumerable<NonInventoryItem> GetInsertedRow(int LastItemBefore, int LastItemAfter);
        IEnumerable<NonInventoryItem> GetAll();
        NonInventoryItem GetDetailsByItemCode(string itemcode);
        IEnumerable<NonInventoryItem> GetItemsByCategory(string cat);
        IEnumerable<NonInventoryItem> GetItemsBySubcategory(string subcat);
        IEnumerable<NonInventoryItem> GetItemsByBrand(string Brand);
        NonInventoryItem GetItemById(int id);
    }
}
