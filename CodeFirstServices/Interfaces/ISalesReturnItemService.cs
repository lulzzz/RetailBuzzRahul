using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesReturnItemService
    {
        void Create(SalesReturnItem SalesReturnItem);
        IEnumerable<SalesReturnItem> GetAllNewItemsByBill(string billno);
        IEnumerable<SalesReturnItem> GetAllItemsBySales(string salesreturnno);
        IEnumerable<SalesReturnItem> GetItemListBySalesReturnNo(string salesreturnno);
        IEnumerable<SalesReturnItem> GetItemListByBillNo(string no);
    }
}
