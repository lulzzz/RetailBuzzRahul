using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IItemNameService
    {
        void Create(ItemName Item);
        void Update(ItemName Item);
        ItemName GetLastItemName();
        IEnumerable<ItemName> GetItemNameList(string name);
        ItemName GetItemNamebyId(int id);
        IEnumerable<ItemName> GetItemNameListBySubcategory(string subcat, string item);
        IEnumerable<ItemName> GetItemBySubCategory(string subcat);
        IEnumerable<ItemName> GetItemBySubCategoryForDelete(string subcat);
        ItemName GetInventoryLastItemName();
        ItemName GetNonInventoryLastItemName();
        ItemName GetDataByItemName(string item);
        IEnumerable<ItemName> GetItemNameForDelete();
        ItemName GetId(int id);
        IEnumerable<ItemName> GetItemNames();
        IEnumerable<ItemName> GetitemName(string term);
        ItemName GetItemIdByName(string name);
    }
}
