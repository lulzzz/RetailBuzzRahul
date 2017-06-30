using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInventoryTaxService
    {
        void Create(InventoryTax InventoryTax);
        void Update(InventoryTax InventoryTax);
        void Delete(InventoryTax InventoryTax);
        IEnumerable<InventoryTax> GetTaxesByCode(string code);
        InventoryTax GetTaxById(int id);
    }
}
