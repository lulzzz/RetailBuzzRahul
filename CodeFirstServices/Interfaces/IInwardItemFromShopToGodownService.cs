using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
   public interface IInwardItemFromShopToGodownService
    {
        void Create(InwardItemFromShopToGodown Inward);
        void Update(InwardItemFromShopToGodown Inward);
        IEnumerable<InwardItemFromShopToGodown> GetItemsByInwardNo(string InwardNo);
    }
}
