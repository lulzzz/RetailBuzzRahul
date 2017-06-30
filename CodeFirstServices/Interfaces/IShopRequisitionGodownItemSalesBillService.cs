using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public interface IShopRequisitionGodownItemSalesBillService
    {
      ShopRequisitionGodownitemsalesbill getAllItems(string itemcode);
      void CreateShopRequisitionGodownItem(ShopRequisitionGodownitemsalesbill shoprequisitiongodownitem);
      IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemDetailsByReqCode(string code);
      IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname, string requisitionfromshopname);
      IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(string code, DateTime? date, string godownname, string requiusitionfromgodownname);
      IEnumerable<ShopRequisitionGodownitemsalesbill> getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(string code, DateTime? date, string shopname, string requisitionfromshopname);
      ShopRequisitionGodownitemsalesbill GetDetailsByItemCodeAndFromShopName(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      void Update(ShopRequisitionGodownitemsalesbill shoprequisitiongodown);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      ShopRequisitionGodownitemsalesbill GetDetailsByItemCodeAndFromShopNameForGodownLogin(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownName);

      IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat);
      IEnumerable<ShopRequisitionGodownitemsalesbill> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode);
  }
}
