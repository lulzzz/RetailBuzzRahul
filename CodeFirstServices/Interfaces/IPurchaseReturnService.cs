using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseReturnService
    {
        void Create(PurchaseReturn PurchaseReturn);
        void Update(PurchaseReturn PurchaseReturn);
        PurchaseReturn GetLastRow();
        PurchaseReturn GetLastPurchaseByFinYr(string year, string code);
        PurchaseReturn GetDetailsById(int id);
        PurchaseReturn GetLastReturnBillOfSupplier(string inwardno);
        PurchaseReturn GetPurchaseByReturnNo(string prno);
        IEnumerable<PurchaseReturn> GetAllPurchaseReturns(string prno);
        IEnumerable<PurchaseReturn> GetDailyPurchaseReturns();
    }
}
