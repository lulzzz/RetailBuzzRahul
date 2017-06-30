using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;

namespace CodeFirstServices.Services
{
    public class ItemSubCategoryService : IItemSubCategoryService
    {
        private readonly IItemSubCategoryRepository _ItemSubCategoryRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ItemSubCategoryService(IItemSubCategoryRepository ItemSubCategoryRepository, IUnitOfWork UnitOfWork)
        {
            this._ItemSubCategoryRepository = ItemSubCategoryRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void CreateItemCatgory(ItemSubCategory itemSubCategory)
        {
            _ItemSubCategoryRepository.Add(itemSubCategory);
            _UnitOfWork.Commit();
        }

        public void UpdateItemCatgory(ItemSubCategory itemSubCategory)
        {
            _ItemSubCategoryRepository.Update(itemSubCategory);
            _UnitOfWork.Commit();
        }
        public IEnumerable<ItemSubCategory> getSubCategory()
        {
            var SubCategory = _ItemSubCategoryRepository.GetAll();
            return SubCategory;
        }

        public ItemSubCategory GetSubCAtegorybyId(int id)
        {
            var SubCategory = _ItemSubCategoryRepository.Get(c => c.subCategoryId == id);
            return SubCategory;
        }

        public IEnumerable<ItemSubCategory> GetSubCategoryByCategory(string type)
        {
            var List = _ItemSubCategoryRepository.GetMany(sup => sup.categoryName == type && sup.status == "Active");
            return List;
        }

        public ItemSubCategory GetId(int id)
        {
            var list = _ItemSubCategoryRepository.GetById(id);
            return list;
        }

        public IEnumerable<ItemSubCategory> GetItemSubCategories()
        {
            var list = _ItemSubCategoryRepository.GetMany(sta => sta.status == "Active");
            return list;
        }

        public IEnumerable<ItemSubCategory> GetItemSubCategoryList(string name)
        {
            var list = _ItemSubCategoryRepository.GetMany(l => l.subCategoryName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.subCategoryName);
            return list;
        }
        public IEnumerable<ItemSubCategory> GetSubCategoryName(string catname, string subcatname)
        {
            var namelist = _ItemSubCategoryRepository.GetMany(n => n.subCategoryName == subcatname && n.categoryName == catname);
            return namelist;
        }

        public ItemSubCategory getLastItemSubCategory()
        {
            var lastrow = _ItemSubCategoryRepository.GetAll().LastOrDefault();
            return lastrow;
        }

        public IEnumerable<ItemSubCategory> GetSubCatByCatCode(string name)
        {
            var list = _ItemSubCategoryRepository.GetMany(l => l.categoryName == name);
            return list;
        }

        public string GetCategoryBySubCat(string subcat)
        {
            string data = _ItemSubCategoryRepository.Get(l => l.subCategoryName == subcat).categoryName;
            return data;
        }

        public IEnumerable<ItemSubCategory> GetActiveSubCategories(string subcat)
        {
            var cl1 = _ItemSubCategoryRepository.GetMany(cl => cl.subCategoryName.ToLower().StartsWith(subcat.ToString().ToLower()) && cl.status == "Active");
            return cl1;
        }

        public IEnumerable<ItemSubCategory> GetSubcategoryList(string term, string Cat)
        {
            var cl1 = _ItemSubCategoryRepository.GetMany(cl => cl.subCategoryName.ToLower().StartsWith(term.ToString().ToLower()) && cl.status == "Active" && cl.categoryName == Cat);
            return cl1;
        }
    }
}
