using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
 public   interface IItemCategoryService
    {
     void CreateItemCatgory(ItemCategory itemCategory);
     void UpdateItemCatgory(ItemCategory itemCategory);
     ItemCategory getById(int id);
     //IEnumerable<ItemCategory> getCategory();
     IEnumerable<ItemCategory> GetCategoryforList(string Category);
     IEnumerable<ItemCategory> GetItemCategories();
     ItemCategory GetId(int id);
     IEnumerable<ItemCategory> GetItemCategoryList(string name);
     IEnumerable<ItemCategory> GetCategoryName(string name);
     ItemCategory getLastItemCategory();
     IEnumerable<ItemCategory> GetAllItemCategories();
     string GetCodeByName(string name);
    }
}
