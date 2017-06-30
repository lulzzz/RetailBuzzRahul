using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
 public interface IShopRequisitionGodownService
    {
     ShopRequisitionGodown GetLastShopRequisition();
     void CreateShopRequisitionGodown(ShopRequisitionGodown shoprequisitiongodown);
     ShopRequisitionGodown getShopRequisitionDetailsByShopCode(string code);
     IEnumerable<ShopRequisitionGodown> GetREList(string term);
     ShopRequisitionGodown getShopRequisitionDetailsByShopName(string name);
     IEnumerable<ShopRequisitionGodown> GetListByShopCode(string code);
     IEnumerable<ShopRequisitionGodown> getListByPo(string value);
     IEnumerable<ShopRequisitionGodown> getAllDetails();
    }
}
