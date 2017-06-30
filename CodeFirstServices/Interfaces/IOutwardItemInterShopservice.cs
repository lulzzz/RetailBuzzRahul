using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeFirstServices.Interfaces
{
  public interface IOutwardItemInterShopservice
    {
      void CreateInterShopitem(OutwardItemInterShop outwarditem);
      IEnumerable<OutwardItemInterShop> GetDetailsByOutwardCode(string code);
      OutwardItemInterShop GetDetailsByItemCodeandShopCode(string itemcode, string shopcode);
      void Update(OutwardItemInterShop outwarditem);
      void Delete(OutwardItemInterShop outwarditem);
    }
}
