using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
   public  class ShopRequisitionGodownItemService:IShopRequisitionGodownItemService
   {
       private readonly IShopRequisitionGodownItemRepository _shoprequisitionrepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShopRequisitionGodownItemService(IShopRequisitionGodownItemRepository shoprequisitionrepository, IUnitOfWork unitOfWork)
        {
            this._shoprequisitionrepository = shoprequisitionrepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateShopRequisitionGodownItem(ShopRequisitionGodownItem shoprequisitiongodownitem)
        {
            _shoprequisitionrepository.Add(shoprequisitiongodownitem);
            _unitOfWork.Commit();

        }

        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByReqCode(string code)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.ReqCode == code);
            return item;
        }

        public ShopRequisitionGodownItem getAllItems(string itemcode)
        {
            var item = _shoprequisitionrepository.Get(it=>it.ItemCode==itemcode);
            return item;
        }

        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByGodownName(string name)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.RequisitionToGodownName==name);
            return item;
        }

        public void Update(ShopRequisitionGodownItem shoprequisitiongodown)
        {
            _shoprequisitionrepository.Update(shoprequisitiongodown);
            _unitOfWork.Commit();
        }

        public ShopRequisitionGodownItem GetDetailsByItemCodeAndName(string itemcode, string name)
        {
            var data = _shoprequisitionrepository.Get(d => d.ItemCode == itemcode && d.RequisitionToGodownName == name);
            return data;
        }

        public ShopRequisitionGodownItem GetDetailsByItemCodeAndFromShopName(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _shoprequisitionrepository.Get(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToShopName == requisitionfromshopName);
            return data;
        }


        public ShopRequisitionGodownItem GetDetailsByItemCodeAndFromShopNameForGodownLogin(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _shoprequisitionrepository.Get(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToGodownName == requisitionfromgodownName);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
           var data = _shoprequisitionrepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToShopName == requisitionfromshopName);
           return data;
        }
        public IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _shoprequisitionrepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToGodownName == requisitionfromgodownName);
            return data;
        }
        public IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _shoprequisitionrepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToShopName == requisitionfromshopName).Skip(1);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _shoprequisitionrepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionToGodownName == requisitionfromgodownName).Skip(1);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByRequisitionFromShopName(string name)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.FromShopName==name);
            return item;
        }

        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname, string requisitionfromshopname)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName==shopname);
            return item;
        }

        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(string code, DateTime? date, string shopname, string requisitionfromshopname)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName == shopname && co.RequisitionToShopName==requisitionfromshopname);
            return item;
        }
        public IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(string code, DateTime? date, string godownname,string requiusitionfromgodownname)
        {
            var item = _shoprequisitionrepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName == godownname && co.RequisitionToGodownName==requiusitionfromgodownname);
            return item;
        }

        public IEnumerable<ShopRequisitionGodownItem> getallitems()
        {
            var item = _shoprequisitionrepository.GetAll();
            return item;
        }


        public IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate)
        {
            var data = _shoprequisitionrepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat)
        {
            var data = _shoprequisitionrepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date && c.SubCategory == subcat);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode)
        {
            var data = _shoprequisitionrepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date && c.ItemCode == itemcode);
            return data;
        }
    }
    }

