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
    public class GodownStockService:IGodownStockService
    {
        public readonly IGodownStockRepository _godownstockrepository;
        private readonly IUnitOfWork _unitofwork;
        public GodownStockService(IGodownStockRepository godownstockrepository, IUnitOfWork unitofwork)
        {
            this._godownstockrepository = godownstockrepository;
            this._unitofwork = unitofwork;
        }

        public void Create(GodownStock godownstock)
        {
            _godownstockrepository.Add(godownstock);
            _unitofwork.Commit();
        }

        public void Update(GodownStock godownstock)
        {
            _godownstockrepository.Update(godownstock);
            _unitofwork.Commit();
        }

        public IEnumerable<GodownStock> GetGodownStockTillDate()
        {
            var list = _godownstockrepository.GetAll();
            return list;
        }

        public GodownStock GetDetailsByItemCodeAndGodownCode(string itemcode, string godowncode)
        {
            var data = _godownstockrepository.Get(d => d.ItemCode == itemcode && d.GodownCode == godowncode);
            return data;
        }

        public IEnumerable<GodownStock> GetDetailsByItemCodeAndGodownCodeForDynamicList(string itemcode, string godowncode)
        {
            var data = _godownstockrepository.GetMany(d => d.ItemCode == itemcode && d.GodownCode == godowncode);
            return data;
        }

        public IEnumerable<GodownStock> GetRowsByItemCode(string itemcode)
        {
            var rows = _godownstockrepository.GetMany(r => r.ItemCode == itemcode);
            return rows;
        }

        public IEnumerable<GodownStock> GetRowsByGodownCode(string code)
        {
            var rows = _godownstockrepository.GetMany(r => r.GodownCode == code);
            return rows;
        }

        public IEnumerable<GodownStock> getGodownStock()
        {
            var stock = _godownstockrepository.GetMany(m => m.Status == "Active");
            return stock;
        }

        public IEnumerable<GodownStock> GetRowsByGodownName(string name)
        {
            var rows = _godownstockrepository.GetMany(r => r.GodownName == name).OrderBy(r=>r.Quantity);
            return rows;
        }

        public IEnumerable<GodownStock> GetRowsByGodowCode(string code)
        {
            var rows = _godownstockrepository.GetMany(r => r.GodownCode == code && r.Quantity<=12 ).OrderBy(r => r.Quantity);
            return rows;
        }

        public IEnumerable<GodownStock> GetRowsByGodownItemName(string name)
        {
            var rows = _godownstockrepository.GetMany(r => r.ItemName == name);
            return rows;
        }

        public IEnumerable<GodownStock> GetNamesByItem(string name, string godownname)
        {
            var namelist = _godownstockrepository.GetMany(itemname => itemname.GodownName == godownname && itemname.ItemName.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return namelist;
        }

        public IEnumerable<GodownStock> getGodownStockNameByItemName(string name)
        {
            var list = _godownstockrepository.GetMany(itemname => itemname.ItemName.ToLower().Contains(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return list; 
        }

        public IEnumerable<GodownStock> GetRowsByItemnameandShopName(string itemname, string godownname)
        {
            var data = _godownstockrepository.GetMany(d => d.ItemName == itemname && d.GodownName == godownname);
            return data;
        }

        public IEnumerable<GodownStock> getItemDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var data = _godownstockrepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate);
            return data;
        }

        public IEnumerable<GodownStock> getItemDetailsByDateAndItemName(DateTime fromdate, DateTime todate, string itemname)
        {
            var data = _godownstockrepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate && c.ItemName == itemname);
            return data;
        }

        public GodownStock GetRowsByItemCodeandShopName(string code, string godownname)
        {
            var data = _godownstockrepository.Get(d => d.ItemCode == code && d.GodownName == godownname);
            return data;
        }

        public IEnumerable<GodownStock> GetRowsByItemcodeandItemname(string itemcode, string itemname,string godownname)
        {
            var data = _godownstockrepository.GetMany(d => d.ItemCode == itemcode && d.ItemName == itemname && d.GodownName==godownname);
            return data;
        }
        public IEnumerable<GodownStock> GetRowsByItemcodeandItemnameWithoutGodownName(string itemcode, string itemname)
        {
            var data = _godownstockrepository.GetMany(d => d.ItemCode == itemcode && d.ItemName == itemname);
            return data;
        }






        
    }
}
