using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IShopRequisitionGodownItemService
    {
        void CreateShopRequisitionGodownItem(ShopRequisitionGodownItem shoprequisitiongodownitem);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByReqCode(string code);
        ShopRequisitionGodownItem getAllItems(string itemcode);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByGodownName(string name);
        void Update(ShopRequisitionGodownItem shoprequisitiongodown);
        ShopRequisitionGodownItem GetDetailsByItemCodeAndName(string itemcode, string name);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemDetailsByRequisitionFromShopName(string name);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname, string requisitionfromshopname);
        IEnumerable<ShopRequisitionGodownItem> getallitems();
        //ShopRequisitionGodownItem GetDetailsByItemCodeAndFromShopName(string itemcode, string name, DateTime? date);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(string code, DateTime? date, string godownname,string requiusitionfromgodownname);
        ShopRequisitionGodownItem GetDetailsByItemCodeAndFromShopName(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
        // IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopName(string itemcode, string name, DateTime? date);
        IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
        IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
        ShopRequisitionGodownItem GetDetailsByItemCodeAndFromShopNameForGodownLogin(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);
        IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);
        IEnumerable<ShopRequisitionGodownItem> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);
        IEnumerable<ShopRequisitionGodownItem> getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(string code, DateTime? date, string shopname, string requisitionfromshopname);

        IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate);
        IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat);
        IEnumerable<ShopRequisitionGodownItem> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode);
    }
}

