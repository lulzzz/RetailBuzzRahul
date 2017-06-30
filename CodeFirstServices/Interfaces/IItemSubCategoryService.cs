using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IItemSubCategoryService
    {
        void CreateItemCatgory(ItemSubCategory itemSubCategory);
        void UpdateItemCatgory(ItemSubCategory itemSubCategory);
        IEnumerable<ItemSubCategory> getSubCategory();
        ItemSubCategory GetSubCAtegorybyId(int id);
        IEnumerable<ItemSubCategory> GetSubCategoryByCategory(string type);
        ItemSubCategory GetId(int id);
        IEnumerable<ItemSubCategory> GetItemSubCategories();
        IEnumerable<ItemSubCategory> GetItemSubCategoryList(string name);
        IEnumerable<ItemSubCategory> GetSubCategoryName(string catname, string subcatname);
        ItemSubCategory getLastItemSubCategory();
        IEnumerable<ItemSubCategory> GetSubCatByCatCode(string name);
        string GetCategoryBySubCat(string subcat);
        IEnumerable<ItemSubCategory> GetActiveSubCategories(string subcat);
        IEnumerable<ItemSubCategory> GetSubcategoryList(string term, string Cat);
    }
}
