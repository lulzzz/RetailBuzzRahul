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
    public class ItemNameService : IItemNameService
    {
        private readonly IItemNameRepository _itemnameRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemNameService(IItemNameRepository itemnameRepository, IUnitOfWork unitOfWork)
        {
            this._itemnameRepository = itemnameRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(ItemName Item)
        {
            _itemnameRepository.Add(Item);
            _unitOfWork.Commit();
        }

        public void Update(ItemName Item)
        {
            _itemnameRepository.Update(Item);
            _unitOfWork.Commit();
        }

        public ItemName GetLastItemName()
        {
            var itemData = _itemnameRepository.GetAll().LastOrDefault();
            return itemData;
        }

        public IEnumerable<ItemName> GetItemNameList(string name)
        {
            var list = _itemnameRepository.GetMany(l => l.Name.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.Name);
            return list;
        }

        public ItemName GetItemNamebyId(int id)
        {
            var data = _itemnameRepository.Get(c => c.Id == id);
            return data;
        }

        public IEnumerable<ItemName> GetItemNameListBySubcategory(string subcat, string item)
        {
            var namelist = _itemnameRepository.GetMany(n => n.ItemSubcategory == subcat && n.Name == item);
            return namelist;
        }

        public IEnumerable<ItemName> GetItemBySubCategory(string subcat)
        {
            var List = _itemnameRepository.GetMany(sup => sup.ItemSubcategory == subcat && sup.Status == "Active");
            return List;
        }

        public IEnumerable<ItemName> GetItemBySubCategoryForDelete(string subcat)
        {
            var List = _itemnameRepository.GetMany(sup => sup.ItemSubcategory == subcat && sup.DeleteStatus == "Active");
            return List;
        }

        public ItemName GetInventoryLastItemName()
        {
            var itemData = _itemnameRepository.GetMany(d => d.ItemType == "Inventory").LastOrDefault();
            return itemData;
        }

        public ItemName GetNonInventoryLastItemName()
        {
            var itemData = _itemnameRepository.GetMany(d => d.ItemType == "NonInventory").LastOrDefault();
            return itemData;
        }

        public ItemName GetDataByItemName(string item)
        {
            var data = _itemnameRepository.Get(n => n.Name == item);
            return data;
        }

        public IEnumerable<ItemName> GetItemNameForDelete()
        {
            var List = _itemnameRepository.GetMany(sup => sup.Status == "Active");
            return List;
        }

        public ItemName GetId(int id)
        {
            var list = _itemnameRepository.GetById(id);
            return list;
        }

        public IEnumerable<ItemName> GetItemNames()
        {
            var data = _itemnameRepository.GetAll();
            return data;
        }

        public IEnumerable<ItemName> GetitemName(string term)
        {
            var details = _itemnameRepository.GetMany(d => d.Name.ToLower().StartsWith(term.ToString().ToLower()));
            return details;
        }

        public ItemName GetItemIdByName(string name)
        {
            var data = _itemnameRepository.Get(c => c.Name == name);
            return data;
        }
    }
}
