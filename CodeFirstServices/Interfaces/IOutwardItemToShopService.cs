using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardItemToShopService
    {
        void Create(OutwardItemToShop godown);
        void Update(OutwardItemToShop godown);
        IEnumerable<OutwardItemToShop> GetDetailsByOutwardNo(string outno);
        OutwardItemToShop GetDetailsByOutwardCode(string code);
        OutwardItemToShop getLastInsertedOutward();
        void Delete(OutwardItemToShop outwarditem);
    }
}
