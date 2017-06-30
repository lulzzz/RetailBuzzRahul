using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTruncateRepository _itemTruncateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IItemRepository itemRepository, IUnitOfWork unitOfWork, IItemTruncateRepository itemTruncateRepository)
        {
            this._itemRepository = itemRepository;
            this._unitOfWork = unitOfWork;
            this._itemTruncateRepository = itemTruncateRepository;
        }


        public IEnumerable<Item> getAllItems()
        {
            var item = _itemRepository.GetMany(item1 => item1.status == "Active");
            return item;
        }

        public void createItem(Item item)
        {
            _itemRepository.Add(item);
            _unitOfWork.Commit();
        }

        public void updateItem(Item item)
        {
            _itemRepository.Update(item);
            _unitOfWork.Commit();
        }

        public Item getById(int id)
        {
            var itemid = _itemRepository.GetById(id);
            return itemid;
        }

        public IEnumerable<Item> GetItemByCategory(string category, string subCategory)
        {
            var itemData = _itemRepository.GetMany(itm => itm.itemCategory == category && itm.itemSubCategory == subCategory && Convert.ToDateTime(itm.modifedOn).Date == DateTime.Now.Date);
            return itemData;
        }

        public IEnumerable<Item> GetItemByCategoryForRequisition(string category, string subCategory)
        {
            var itemData = _itemRepository.GetMany(itm => itm.itemCategory == category && itm.itemSubCategory == subCategory );
            return itemData;
        }


        public Item GetLastItem()
        {
            var itemData = _itemRepository.GetAll().LastOrDefault();
            return itemData;
        }

        public IEnumerable<Item> GetItemNames(string name)
        {
            var itemlist = _itemRepository.GetMany(itmname => itmname.itemName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(itmname => itmname.itemName);
            return itemlist;
        }

        public IEnumerable<Item> GetBrandNames(string name)
        {
            var itemlist = _itemRepository.GetMany(itmname => itmname.brandName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(itmname => itmname.brandName);
            return itemlist;
        }
        ////public IEnumerable<Item> GetItemByNamesAndBrandName(string name,string brandname)
        ////{
        ////    var itemlist = _itemRepository.GetMany(itmname => itmname.itemName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(itmname => itmname.itemName);
        ////    return itemlist;
        ////}

        public Item GetDescriptionByItemCode(string code)
        {
            var details = _itemRepository.Get(item => item.itemCode == code);
            return details;
        }

        public Item GetDescriptionByItemAndBrandName(string name, string brandname)
        {
            var details = _itemRepository.Get(item => item.itemName == name && item.brandName==brandname);
            return details;
        }

        public Item GetDescriptionByItemNameandItemCodeandBrandName(string itemcode, string itemname, string brandname)
        {
            var details = _itemRepository.Get(item => item.itemCode==itemcode && item.itemName==itemname && item.brandName==brandname);
            return details;
        }
        public Item GetDescriptionByItemNameandItemCode(string itemcode, string itemname)
        {
            var details = _itemRepository.Get(item => item.itemCode == itemcode && item.itemName == itemname);
            return details;
        }

        public IEnumerable<Item> GetSubCatByCatCode(string name)
        {
            var list = _itemRepository.GetMany(l => l.itemCategory == name);
            return list;
        }

        public IEnumerable<Item> GetItemsByBrand(string Brand)
        {
            var list = _itemRepository.GetMany(l => l.brandName == Brand);
            return list;
        }

        public string GetItemNameById(int Id)
        {
            var name = _itemRepository.Get(n => n.itemId == Id);
            return name.itemName;
        }

        public List<ItemTruncate> ExecuteProcedure(string procname)
        {
            var data = _itemTruncateRepository.ExecuteCustomStoredProc(procname);
            return data;
        }

        public Item CheckForData(BarcodeTempDetail Data)
        {
            var Details = _itemRepository.Get(x => x.itemName == Data.itemName && x.itemCategory == Data.itemCategory && x.itemSubCategory == Data.itemSubCategory
                && x.colorCode == Data.colorCode && x.designCode == Data.designCode && x.typeOfMaterial == Data.typeOfMaterial);
            return Details;
        }

        public IEnumerable<Item> GetItemsBySubCategory(string type)
        {
            var List = _itemRepository.GetMany(s => s.itemSubCategory == type && s.status == "Active");
            return List;
        }

        public IEnumerable<Item> GetInsertedRow(int LastItemBefore, int LastItemAfter)
        {
            var list = _itemRepository.GetMany(l => l.itemId >= LastItemBefore && l.itemId <= LastItemAfter);
            return list;
        }

        public Item GetItem(int LastItemBefore)
        {
            var list = _itemRepository.Get(l => l.itemId == LastItemBefore);
            return list;
        }

        public IEnumerable<Item> GetItemsBySubCategoryAndItemType(string subcat)
        {
            var List = _itemRepository.GetMany(s => s.itemSubCategory == subcat && s.itemtype=="Inventory" && s.status == "Active");
            return List;
        }

        public IEnumerable<Item> GetAllItems()
        {
            var list = _itemRepository.GetAll();
            return list;
        }

        public Item GetDesignNameByDesignCode(string designcode)
        {
            var data = _itemRepository.Get(m => m.designCode == designcode);
            return data;
        }

        public Item GetDetailsByBarcode(string barcode)
        {
            var details = _itemRepository.Get(item => item.Barcode == barcode);
            return details;
        }

        public IEnumerable<Item> GetItemsByCategory(string type)
        {
            var List = _itemRepository.GetMany(s => s.itemCategory == type && s.status == "Active");
            return List;
        }

        public Item GetItemName(string itemname)
        {
            var detaila = _itemRepository.Get(item => item.itemName == itemname);
            return detaila;
        }

        public Item GetcolorCodebyColorcode(string colorcode)
        {
            var details = _itemRepository.Get(item => item.colorCode == colorcode);
            return details;
        }


        public Item GetItemByUniversalBarcode(string UniversalBarcode)
        {
            var details = _itemRepository.Get(it => it.UniversalBarcode == UniversalBarcode);
            return details;
        }
    }
}
