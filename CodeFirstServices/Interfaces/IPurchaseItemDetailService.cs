using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseItemDetailService
    {
        void CreatePurchaseItemDetail(PurchaseItemDetail PurchaseDetail);
        IEnumerable<PurchaseItemDetail> GetPurchaseItemsByPONo(string PoNo);
        void UpdatePurchaseItemDetail(PurchaseItemDetail PurchaseDetail);
        void DeletePurchaseItemDetail(PurchaseItemDetail PurchaseDetail);
        PurchaseItemDetail GetDetailsById(int id);
        IEnumerable<PurchaseItemDetail> GetPurchaseInventoryItemsByPONo(string PoNo);
        IEnumerable<PurchaseItemDetail> GetReportByItemAndDate(string Item,DateTime fromdate, DateTime todate);
    }
}
