using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IItemService
    {
        IEnumerable<Item> getAllItems();
        void createItem(Item item);
        void updateItem(Item item);
        Item getById(int id);
        IEnumerable<Item> GetItemByCategory(string category, string subCategory);
        Item GetLastItem();
        IEnumerable<Item> GetItemNames(string name);
        Item GetDescriptionByItemCode(string code);
        IEnumerable<Item> GetSubCatByCatCode(string name);
        IEnumerable<Item> GetItemsByBrand(string Brand);
        string GetItemNameById(int Id);
        List<ItemTruncate> ExecuteProcedure(string procname);
        Item CheckForData(BarcodeTempDetail Data);
        IEnumerable<Item> GetItemsBySubCategory(string type);
        IEnumerable<Item> GetInsertedRow(int LastItemBefore, int LastItemAfter);
        Item GetItem(int LastItemBefore);
        IEnumerable<Item> GetItemsBySubCategoryAndItemType(string subcat);
        IEnumerable<Item> GetAllItems();
        Item GetDesignNameByDesignCode(string designcode);
        IEnumerable<Item> GetItemByCategoryForRequisition(string category, string subCategory);
        Item GetDetailsByBarcode(string barcode);
        IEnumerable<Item> GetItemsByCategory(string type);
        Item GetDescriptionByItemAndBrandName(string name, string brandname);
        IEnumerable<Item> GetBrandNames(string name);
        Item GetDescriptionByItemNameandItemCodeandBrandName(string itemcode, string itemname, string brandname);
        Item GetDescriptionByItemNameandItemCode(string itemcode, string itemname);
        Item GetItemName(string itemname);
        Item GetcolorCodebyColorcode(string colorcode);
        Item GetItemByUniversalBarcode(string UniversalBarcode);
    }
}
