using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
  public interface IRequisitionForGodownService
    {
      void CreateRequisitionForGodown(RequisitionForGodown requisitionforgodown);
      IEnumerable<RequisitionForGodown> GetRequisitionsForPo(string value);
      IEnumerable<RequisitionForGodown> getItemsIfRejectQuantityIsNotNull();
      IEnumerable<RequisitionForGodown> GetRowsByItemnameandGodownNameForRequisitionForGodown(string itemname, string godownname);
      IEnumerable<RequisitionForGodown> getItemDetailsByDateForRequisitionForGodown(DateTime fromdate, DateTime todate);
      IEnumerable<RequisitionForGodown> getItemDetailsByDateAndItemNameForRequisitionForGodown(DateTime fromdate, DateTime todate, string itemname);
      IEnumerable<RequisitionForGodown> getDetailsOfToGodownIfNotNull();
      IEnumerable<RequisitionForGodown> getRequisitionForGodownDetailsiItemCodeAndDate(string code, DateTime? date, string godownname, string togodownname);
      RequisitionForGodown GetDetailsByIcAndfsAndDTAndRSForGodownUpdate(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownname);
      IEnumerable<RequisitionForGodown> GetDetailsByICAndFShNameIenumerableForRequisitionOfgodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      void Update(RequisitionForGodown requisitionforgodown);
      IEnumerable<RequisitionForGodown> GetDetailsByICodeAndSNIenumerableSkipFirstForGodownUpdate(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
      IEnumerable<RequisitionForGodown> GetNamesByItemForRequisitionForGodown(string name, string shopname);
      RequisitionForGodown GetLastRequisition();
      IEnumerable<RequisitionForGodown> GetDailyRequisitionsForGodowns(string loggedinshopname);
      IEnumerable<RequisitionForGodown> GetDailyRequisitionsForSuperAdmin(string loggedinshopname);
      IEnumerable<RequisitionForGodown> getAllActiveRequisitions();

      IEnumerable<RequisitionForGodown> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate);
      IEnumerable<RequisitionForGodown> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat);
      IEnumerable<RequisitionForGodown> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode);
  }
}
 