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
    public class NonInventoryItemService : INonInventoryItemService
    {
        private readonly INonInventoryItemRepository _NonInventoryitemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NonInventoryItemService(INonInventoryItemRepository itemRepository, IUnitOfWork unitOfWork)
        {
            this._NonInventoryitemRepository = itemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void createNonInventoryItem(NonInventoryItem item)
        {
            _NonInventoryitemRepository.Add(item);
            _unitOfWork.Commit();
        }

        public NonInventoryItem GetLastItem()
        {
            var itemData = _NonInventoryitemRepository.GetAll().LastOrDefault();
            return itemData;
        }

        public IEnumerable<NonInventoryItem> GetInsertedRow(int LastItemBefore, int LastItemAfter)
        {
            var list = _NonInventoryitemRepository.GetMany(l => l.itemId >= LastItemBefore && l.itemId <= LastItemAfter);
            return list;
        }
        
        public IEnumerable<NonInventoryItem> GetAll()
        {
            var items = _NonInventoryitemRepository.GetAll();
            return items;
        }

        public NonInventoryItem GetDetailsByItemCode(string itemcode)
        {
            var itemData = _NonInventoryitemRepository.Get(i => i.itemCode == itemcode);
            return itemData;
        }

        public IEnumerable<NonInventoryItem> GetItemsByCategory(string cat)
        {
            var data = _NonInventoryitemRepository.GetMany(s => s.itemCategory == cat);
            return data;
        }

        public IEnumerable<NonInventoryItem> GetItemsBySubcategory(string subcat)
        {
            var data = _NonInventoryitemRepository.GetMany(s => s.itemSubCategory == subcat);
            return data;
        }

        public IEnumerable<NonInventoryItem> GetItemsByBrand(string Brand)
        {
            var list = _NonInventoryitemRepository.GetMany(l => l.brandName == Brand);
            return list;
        }

        public NonInventoryItem GetItemById(int id)
        {
            var data = _NonInventoryitemRepository.GetById(id);
            return data;
        }
    }
}
