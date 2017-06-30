using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ITemporaryCashMemoItemService
    {
        void Create(TemporaryCashMemoItem cashmemoitem);
        void Update(TemporaryCashMemoItem cashmemoitem);
        TemporaryCashMemoItem GetById(int id);
        TemporaryCashMemoItem GetLastItem();
        IEnumerable<TemporaryCashMemoItem> GetAll();
        IEnumerable<TemporaryCashMemoItem> GetDetailsByInvoiceNo(string no);
        IEnumerable<TemporaryCashMemoItem> GetDetailsByRetailBillNo(string code);
        TemporaryCashMemoItem GetItemDetailsByBillNoAndItemCode(string code, string itemcode);
        IEnumerable<TemporaryCashMemoItem> GetDetailsByInvoiceNoAndStatus(string no);
        IEnumerable<TemporaryCashMemoItem> GetItemsByCodeAndType(string no);
       
    }
}
