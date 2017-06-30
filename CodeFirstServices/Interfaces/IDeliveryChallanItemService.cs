using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IDeliveryChallanItemService
    {
        void Create(DeliveryChallanItem DeliveryChallanItem);
        void Update(DeliveryChallanItem DeliveryChallanItem);
        void Delete(DeliveryChallanItem DeliveryChallanItem);
        DeliveryChallanItem GetById(int id);
        IEnumerable<DeliveryChallanItem> GetDetailsByChallanNo(string no);
        double GetAllQuantityByItemCode(string itemcode);
        IEnumerable<DeliveryChallanItem> GetDetailsByQuotNo(string QuotNo);
        DeliveryChallanItem GetDetailsByItemCodeAndQuot(string itemcode, string quotno);
        IEnumerable<DeliveryChallanItem> GetRowsByItemCodeAndQuot(string itemcode, string quotno);
        DeliveryChallanItem GetDetailsByQuotation(string quotno);
        IEnumerable<GetItemsByQuotOrOrderNo> GetItemsByQuotOrOrderNo(string procname, object[] id);
        DeliveryChallanItem GetItemDetailsByItemCodeAndQuotOrOrderNo(string itemcode, string quotno);
        IEnumerable<DeliveryChallanItem> GetDetailsByChallanNoAndStatus(string no);
        DeliveryChallanItem GetDetailsByItemCodeAndChallanNo(string itemcode, string challanno);
        IEnumerable<DeliveryChallanItem> GetInventoryItemsByChallanNo(string no);
        IEnumerable<DeliveryChallanItem> GetAllActiveItemsByChallanNo(string challanno);
    }
}
