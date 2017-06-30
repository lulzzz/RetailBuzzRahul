using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly IItemCategoryRepository _ItemCategoryRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public ItemCategoryService(ItemCategoryRepository ItemCategoryRepository, IUnitOfWork UnitOfWork)
        {
            this._ItemCategoryRepository = ItemCategoryRepository;
            this._UnitOfWork = UnitOfWork;
        }


        public void CreateItemCatgory(ItemCategory itemCategory)
        {
            _ItemCategoryRepository.Add(itemCategory);
            _UnitOfWork.Commit();
        }

        public void UpdateItemCatgory(ItemCategory itemCategory)
        {
            _ItemCategoryRepository.Update(itemCategory);
            _UnitOfWork.Commit();
        }

        public ItemCategory getById(int id)
        {
            var category = _ItemCategoryRepository.Get(it => it.CategoryId == id);
            return category;
        }

        public IEnumerable<ItemCategory> GetCategoryforList(string Category)
        {
            var cl1 = _ItemCategoryRepository.GetMany(cl => cl.CategoryName.ToLower().StartsWith(Category.ToString().ToLower()) && cl.itemCategoryStatus == "Active");
            return cl1;
        }
        public IEnumerable<ItemCategory> GetItemCategories()
        {
            var list = _ItemCategoryRepository.GetMany(itemct => itemct.itemCategoryStatus == "Active");
            return list;
        }
        public ItemCategory GetId(int id)
        {
            var idlist = _ItemCategoryRepository.GetById(id);
            return idlist;
        }
        public IEnumerable<ItemCategory> GetItemCategoryList(string name)
        {
            var list = _ItemCategoryRepository.GetMany(l1 => l1.CategoryName.ToLower().Contains(name.ToString().ToLower())).OrderBy(s => s.CategoryName);
            return list;
        }
        public IEnumerable<ItemCategory> GetCategoryName(string name)
        {
            var namelist = _ItemCategoryRepository.GetMany(n => n.CategoryName == name);
            return namelist;
        }

        public ItemCategory getLastItemCategory()
        {
            var lastrow = _ItemCategoryRepository.GetAll().LastOrDefault();
            return lastrow;
        }

        public IEnumerable<ItemCategory> GetAllItemCategories()
        {
            var data = _ItemCategoryRepository.GetAll();
            return data;
        }

        public string GetCodeByName(string name)
        {
            var codelist = _ItemCategoryRepository.Get(it => it.CategoryName == name);
            return codelist.ItemCategoryCode;
        }
    }
}
