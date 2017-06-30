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
    public class ShopStockService : IShopStockService
    {
        public readonly IShopStockRepository _shopstockrepository;
        private readonly IUnitOfWork _unitofwork;
        public ShopStockService(IShopStockRepository shopstockrepository, IUnitOfWork unitofwork)
        {
            this._shopstockrepository = shopstockrepository;
            this._unitofwork = unitofwork;
        }

        public void Create(ShopStock shopstock)
        {
            _shopstockrepository.Add(shopstock);
            _unitofwork.Commit();
        }

        public void Update(ShopStock shopstock)
        {
            _shopstockrepository.Update(shopstock);
            _unitofwork.Commit();
        }

        public ShopStock GetDetailsByItemCode(string code)
        {
            var data = _shopstockrepository.Get(c => c.ItemCode == code);
            return data;
        }

        public ShopStock GetDetailsByBarcode(string barcode)
        {
            var data = _shopstockrepository.Get(s => s.Barcode == barcode);
            return data;
        }

        public IEnumerable<ShopStock> GetRowsByShopCode(string shopcode)
        {
            var data = _shopstockrepository.GetMany(d => d.ShopCode == shopcode);
            return data;
        }

        public IEnumerable<ShopStock> GetRowsByShopCodeFoRrequsition(string shopcode)
        {
            var data = _shopstockrepository.GetMany(d => d.ShopCode == shopcode && d.Quantity <= 12).OrderBy(d => d.Quantity);
            return data;
        }

        public ShopStock GetDetailsByItemCodeAndShopCode(string itemcode, string shopcode)
        {
            var data = _shopstockrepository.Get(d => d.ItemCode == itemcode && d.ShopCode == shopcode);
            return data;
        }

        public IEnumerable<ShopStock> GetDetailsByItemCodeAndShopCodeForDynamicList(string itemcode, string shopcode)
        {
            var data = _shopstockrepository.GetMany(d => d.ItemCode == itemcode && d.ShopCode == shopcode);
            return data;
        }
        public ShopStock GetLastRowFromItemAndShopCode(string itemcode, string shopcode)
        {
            var data = _shopstockrepository.GetMany(r => r.ItemCode == itemcode && r.ShopCode == shopcode).LastOrDefault();
            return data;
        }

        public ShopStock GetDetailsByItemCodeAndShopName(string itemcode, string shopname)
        {
            var data = _shopstockrepository.Get(r => r.ItemCode == itemcode && r.ShopName == shopname);
            return data;
        }
        public IEnumerable<ShopStock> GetRowsByItemCode(string code)
        {
            var data = _shopstockrepository.GetMany(d => d.ItemCode == code);
            return data;
        }

        public IEnumerable<ShopStock> GetShopStockTillDate()
        {
            var list = _shopstockrepository.GetAll();
            return list;
        }

        public ShopStock GetLastRow()
        {
            var row = _shopstockrepository.GetAll().LastOrDefault();
            return row;
        }

        public ShopStock GetDetailsByShopCode(string code)
        {
            var row = _shopstockrepository.Get(c => c.ShopCode == code);
            return row;
        }

        public IEnumerable<ShopStock> getShopStock()
        {
            var stock = _shopstockrepository.GetMany(m => m.Status == "Active");
            return stock;
        }

        public IEnumerable<ShopStock> GetNamesByItem(string name, string shopname)
        {
            var namelist = _shopstockrepository.GetMany(itemname => itemname.ShopName == shopname && itemname.ItemName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return namelist;
        }

        public IEnumerable<ShopStock> GetRowsByItemnameandShopName(string itemname, string shopname)
        {
            var data = _shopstockrepository.GetMany(d => d.ItemName == itemname && d.ShopName == shopname);
            return data;
        }


        public IEnumerable<ShopStock> GetRowsByItemcodeandItemname(string itemcode, string itemname, string shopname)
        {
            var data = _shopstockrepository.GetMany(d => d.ItemCode == itemcode && d.ItemName == itemname && d.ShopName == shopname);
            return data;
        }

        public IEnumerable<ShopStock> GetRowsByItemcodeandItemnameWithoutshopname(string itemcode, string itemname)
        {
            var data = _shopstockrepository.GetMany(d => d.ItemCode == itemcode && d.ItemName == itemname);
            return data;
        }
        public IEnumerable<ShopStock> getItemDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var data = _shopstockrepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate);
            return data;
        }

        public IEnumerable<ShopStock> getItemDetailsByDateAndItemName(DateTime fromdate, DateTime todate, string itemname)
        {
            var data = _shopstockrepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate && c.ItemName == itemname);
            return data;
        }

        public IEnumerable<ShopStock> getShopStockByItemName(string name)
        {
            var data = _shopstockrepository.GetMany(c => c.ItemName.ToLower().Contains(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return data;
        }

        public IEnumerable<ShopStock> getStockByItemName(string name)
        {
            var data = _shopstockrepository.GetMany(c => c.ItemName == name);
            return data;
        }

    }
}