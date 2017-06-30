using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseInventoryTaxService
    {
        void Create(PurchaseInventoryTax PurchaseInventoryTax);
        void Update(PurchaseInventoryTax PurchaseInventoryTax);
        void Delete(PurchaseInventoryTax PurchaseInventoryTax);
        IEnumerable<PurchaseInventoryTax> GetTaxesByCode(string code);
        PurchaseInventoryTax GetTaxById(int id);
    }
}
