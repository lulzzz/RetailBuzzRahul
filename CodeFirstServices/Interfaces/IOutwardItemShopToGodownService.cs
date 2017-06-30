using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardItemShopToGodownService
    {
        void Create(OutwardItemShopToGodown outward);
        IEnumerable<OutwardItemShopToGodown> GetItemsById(int Id);
        IEnumerable<OutwardItemShopToGodown> GetItemsByOutwardNo(string OutwardNo);
        void Delete(OutwardItemShopToGodown outwarditem);
    }
}
