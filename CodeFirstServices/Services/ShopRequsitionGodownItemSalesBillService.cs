using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
  public class ShopRequsitionGodownItemSalesBillService:IShopRequisitionGodownItemSalesBillService
    {

       private readonly  IShopRequisitionGodownItemSalesBillRepository _ShopRequisitionGodownItemSalesBillRepository;
       private readonly IUnitOfWork _unitOfWork;

        public ShopRequsitionGodownItemSalesBillService(IShopRequisitionGodownItemSalesBillRepository ShopRequisitionGodownItemSalesBillRepository, IUnitOfWork unitOfWork)
        {
            this._ShopRequisitionGodownItemSalesBillRepository = ShopRequisitionGodownItemSalesBillRepository;
            this._unitOfWork = unitOfWork;
        }


        public ShopRequisitionGodownitemsalesbill getAllItems(string itemcode)
        {
            var item = _ShopRequisitionGodownItemSalesBillRepository.Get(it => it.ItemCode == itemcode);
            return item;
        }

        public void CreateShopRequisitionGodownItem(ShopRequisitionGodownitemsalesbill shoprequisitiongodownitem)
        {
            _ShopRequisitionGodownItemSalesBillRepository.Add(shoprequisitiongodownitem);
            _unitOfWork.Commit();

        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemDetailsByReqCode(string code)
        {
            var item = _ShopRequisitionGodownItemSalesBillRepository.GetMany(co => co.ReqCode == code);
            return item;
        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname, string requisitionfromshopname)
        {
            var item = _ShopRequisitionGodownItemSalesBillRepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName == shopname );
            return item;
        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(string code, DateTime? date, string shopname, string requisitionfromshopname)
        {
            var item = _ShopRequisitionGodownItemSalesBillRepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName == shopname && co.RequisitionFromShopName==requisitionfromshopname);
            return item;
        }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(string code, DateTime? date, string godownname, string requiusitionfromgodownname)
        {
            var item = _ShopRequisitionGodownItemSalesBillRepository.GetMany(co => co.ItemCode == code && co.SRDate == date && co.FromShopName == godownname && co.RequisitionFromGodownName == requiusitionfromgodownname);
            return item;
        }


        public ShopRequisitionGodownitemsalesbill GetDetailsByItemCodeAndFromShopName(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.Get(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromShopName == requisitionfromshopName);
            return data;
        }
        public void Update(ShopRequisitionGodownitemsalesbill shoprequisitiongodown)
        {
            _ShopRequisitionGodownItemSalesBillRepository.Update(shoprequisitiongodown);
            _unitOfWork.Commit();
        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromShopName == requisitionfromshopName);
            return data;
        }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromShopName == requisitionfromshopName).Skip(1);
            return data;
        }

        public ShopRequisitionGodownitemsalesbill GetDetailsByItemCodeAndFromShopNameForGodownLogin(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.Get(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromGodownName == requisitionfromgodownName);
            return data;
        }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromGodownName == requisitionfromgodownName);
            return data;
        }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(d => d.ItemCode == itemcode && d.FromShopName == fromshopname && d.SRDate == date && d.RequisitionFromGodownName == requisitionfromgodownName).Skip(1);
            return data;
        }



        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date && c.SubCategory == subcat);
            return data;
        }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode)
        {
            var data = _ShopRequisitionGodownItemSalesBillRepository.GetMany(c => Convert.ToDateTime(c.SRDate).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.SRDate).Date <= Convert.ToDateTime(todate).Date && c.ItemCode == itemcode);
            return data;
        }
    }
}
