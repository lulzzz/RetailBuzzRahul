using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseReturnItemService
    {
        void Create(PurchaseReturnItem PurchaseReturnItem);
        void Update(PurchaseReturnItem PurchaseReturnItem);
        IEnumerable<PurchaseReturnItem> GetItemsByPurchaseReturnNo(string code);
        IEnumerable<PurchaseReturnItem> GetReturnItemsByReturnNo(string returnno);
    }
}
