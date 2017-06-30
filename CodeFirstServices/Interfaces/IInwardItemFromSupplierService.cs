using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardItemFromSupplierService
    {
        void CreateInwardItems(InwardItemsFromSupplier InwardItemsFromSupplier);
        IEnumerable<InwardItemsFromSupplier> GetItemsByPINo(string Pino);
        IEnumerable<ListOfItemCode> GetItemCodesByPoNo(string ProcName, object[] id);
        InwardItemsFromSupplier GetItemDetails(string itemCode, string PoNo);
        IEnumerable<InwardItemsFromSupplier> GetAllQuantity(string itemCode, string PoNo);
        InwardItemsFromSupplier GetItemDetailsByInwardId(int InwardId);
        void DeleteInwardItems(InwardItemsFromSupplier InwardItemsFromSupplier);
        void Update(InwardItemsFromSupplier InwardItemsFromSupplier);
        IEnumerable<InwardItemsFromSupplier> GetInvItemsByInwardNo(string Pino);
        InwardItemsFromSupplier GetItemDetailsByItemCode(string ItemCode);
        InwardItemsFromSupplier GetItemByInwardNoAndItemCode(string inwardno, string itemcode);
        IEnumerable<InwardItemsFromSupplier> GetInvItemsByInwardNoForNopO(string Pino);
    }
}
