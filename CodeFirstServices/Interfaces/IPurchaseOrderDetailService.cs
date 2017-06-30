using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseOrderDetailService
    {
        void CreatePurchaseOrder(PurchaseOrderDetail PurchaseOrderDetail);
        IEnumerable<PurchaseOrderDetail> GetAllPurchaseOrder();
        PurchaseOrderDetail GetPurchaseOrderbyId(int id);
        IEnumerable<PurchaseOrderDetail> GetPendingPoNosBySupplier(string suppliername);
        PurchaseOrderDetail GetDetailByPoNo(string pono);
        IEnumerable<PurchaseOrderDetail> getReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<PurchaseOrderDetail> GetReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate);
        IEnumerable<PurchaseOrderDetail> GetPOesPendingsList(DateTime fromdate, DateTime todate);
        void UpdatePurchaseOrder(PurchaseOrderDetail PurchaseOrderDetail);
        IEnumerable<PurchaseOrderDetail> GetPendingPo(string receivingstatus);
        IEnumerable<PurchaseOrderDetail> GetInwardDetailsByDate();
        IEnumerable<PurchaseOrderDetail> GetPOList(string term);
        PurchaseOrderDetail GetLastPOByFinYr(string year, string godownshopcode);
        IEnumerable<PurchaseOrderDetail> GetPendingPoForCarryForward();
        IEnumerable<PurchaseOrderDetail> GetAllPOesForPrint(string term);
    }
}
