using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public interface IRequisitionForShopService
    {
      void CreateRequisitionForShop(RequisitionForShop requisitionforshop);
      IEnumerable<RequisitionForShop> GetRequisitionsForPo(string value);
      IEnumerable<RequisitionForShop> getRequisitionForShopItemDetailsByGodownName(string name);
      IEnumerable<RequisitionForShop> getDetailsOfToShopIfNotNull();
      IEnumerable<RequisitionForShop> getRequisitionForShopdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname, string toshopname);
      IEnumerable<RequisitionForShop> GetRequisitionDetailsByItemnameandShopName(string itemname, string shopname);
      IEnumerable<RequisitionForShop> getItemDetailsByDateForRequisitionForShop(DateTime? fromdate, DateTime? todate);
      IEnumerable<RequisitionForShop> getItemDetailsByDateAndItemNameForRequisitionForShop(DateTime fromdate, DateTime todate, string itemname);
      IEnumerable<RequisitionForShop> getDetailsOfToGodownIfNotNull();
      IEnumerable<RequisitionForShop> getRequisitionForShopdetailsBiItemCodeAndDateForGodown(string code, DateTime? date, string shopname, string togodownname);
      RequisitionForShop GetDetailsByIcAndfsAndDTAndRS(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      IEnumerable<RequisitionForShop> GetDetailsByICAndFShNameIenumerableForRequisitionOfShop(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      void Update(RequisitionForShop requisitionforshop);
      IEnumerable<RequisitionForShop> GetDetailsByICodeAndSNIenumerableSkipFirstForRequisitionOfShop(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      RequisitionForShop GetDetailsByIcAndfsAndDTAndRSForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownname);
      IEnumerable<RequisitionForShop> GetDetailsByICAndFShNameIenumerableForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      IEnumerable<RequisitionForShop> GetDetailsByICodeAndSNIenumerableSkipFirstForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      IEnumerable<RequisitionForShop> GetNamesByItemForRequisitionForShop(string name, string shopname);
      RequisitionForShop GetLastRequisition();
      RequisitionForShop GetLastRow();
      RequisitionForShop GetDetailsById(int id);
      IEnumerable<RequisitionForShop> GetDailyRequisitionsForShops(string loggedinshopname);
      IEnumerable<RequisitionForShop> GetAll();
      IEnumerable<RequisitionForShop> GetDailyRequisitionsForGodowns(string loggedinshopname);
      IEnumerable<RequisitionForShop> GetDailyRequisitionsForSuperAdmin(string loggedinshopname);
      IEnumerable<RequisitionForShop> getAllActiveRequisitions();
      IEnumerable<GetItemsFromItemMaster> StoredProcedureToFetchItemsFromItemMaster(string procname, object[] id);

      IEnumerable<RequisitionForShop> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate);
      IEnumerable<RequisitionForShop> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat);
      IEnumerable<RequisitionForShop> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode);
  }
}
