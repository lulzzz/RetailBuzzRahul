using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ISalesOrderItemService
    {
        void Create(SalesOrderItem data);
        IEnumerable<SalesOrderItem> GetDetailsByOrderNo(string orderno);
        IEnumerable<GetAllItemsFromStkDistrbtn> GetItemCodeUsingStoredProc(string procname);
        SalesOrderItem GetDetailsById(int id);
        void Update(SalesOrderItem data);
        void Delete(SalesOrderItem data);
        IEnumerable<SalesOrderItem> GetDetailsByQuotNo(string quotno);
        IEnumerable<SalesOrderItem> GetRowsByItemCode(string itemcode);
        IEnumerable<GetItemCodesByQuotAndItemCode> GetItemsByQuotItemCode(string procname, object[] id);
        SalesOrderItem GetDetailsByItemCodeAndQuot(string itemcode, string quotno);
        IEnumerable<SalesOrderItem> GetRowsByItemCodeAndQuot(string itemcode, string quotno);
        IEnumerable<SalesOrderItem> GetDetailsBySONoAndStatus(string orderno);
        SalesOrderItem GetItemDetailsByItemCodeandOrderNo(string itemcode,string orderno);
        IEnumerable<SalesOrderItem> GetDetailsBySONoandItemType(string orderno);
        IEnumerable<SalesOrderItem> GetAllActiveItemsByOrderNo(string orderno);
    }
}
