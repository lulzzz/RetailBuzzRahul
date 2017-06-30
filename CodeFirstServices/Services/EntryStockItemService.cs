using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class EntryStockItemService : IEntryStockItemService
    {
        private readonly IEntryStockItemRepository _EntryStockItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public EntryStockItemService(IEntryStockItemRepository EntryStockItemRepository, IUnitOfWork UnitOfWork)
        {
            this._EntryStockItemRepository = EntryStockItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(EntryStockItem stock)
        {
            _EntryStockItemRepository.Add(stock);
            _UnitOfWork.Commit();
        }

        public void Update(EntryStockItem stock)
        {
            _EntryStockItemRepository.Update(stock);
            _UnitOfWork.Commit();
        }

        public IEnumerable<EntryStockItem> GetAllItems()
        {
            var items = _EntryStockItemRepository.GetAll();
            return items;
        }

        public IEnumerable<EntryStockItem> GetEntryStockItems(string items)
        {
            var data = _EntryStockItemRepository.GetMany(i => i.Item.ToLower().StartsWith(items.ToString().ToLower()));
            return data;
        }

        public EntryStockItem getDetailsByItemName(string itemname)
        {
            var details = _EntryStockItemRepository.Get(it => it.Item == itemname);
            return details;
        }

        public EntryStockItem getDetailsByItemNameandItemCodeAndBrandName(string itemcode,string itemname ,string brandname)
        {
            var details = _EntryStockItemRepository.Get(it => it.ItemCode == itemcode && it.Item == itemname && it.Brand == brandname);
            return details;
        }

        public EntryStockItem getDetailsByItemCode(string itemcode)
        {
            var details = _EntryStockItemRepository.Get(it => it.ItemCode == itemcode);
            return details;
        }

        public IEnumerable<EntryStockItem> GetListByItemCode(string code)
        {
            var data = _EntryStockItemRepository.GetMany(d => d.ItemCode == code);
            return data;
        }

        public IEnumerable<EntryStockItem> GetDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var data = _EntryStockItemRepository.GetMany(d => Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date);
            return data;
        }

        public IEnumerable<EntryStockItem> GetDetailsByCategory(string Category)
        {
            var data = _EntryStockItemRepository.GetMany(d => d.Category==Category);
            return data;
        }

        public IEnumerable<EntryStockItem> GetItemsByQuantity()
        {
            var data = _EntryStockItemRepository.GetMany(d => d.TotalQuantity != '0');
            return data;
        }

        public IEnumerable<EntryStockItem> GetDetailsBySubCategory(string SubCat)
        {
            var data = _EntryStockItemRepository.GetMany(d => d.SubCategory == SubCat);
            return data;
        }

        public IEnumerable<EntryStockItem> GetDetailsByEntryStockCode(string no)
        {
            var list = _EntryStockItemRepository.GetMany(l => l.EntryStockCode == no);
            return list;
        }
    }
}
