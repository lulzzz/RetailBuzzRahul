using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
 public interface IShopRequisitionGodownSalesBillService
    {
     ShopRequisitionGodownsalesbill getShopRequisitionDetailsByShopName(string name);
     void CreateShopRequisitionGodown(ShopRequisitionGodownsalesbill shoprequisitiongodown);
     ShopRequisitionGodownsalesbill GetLastShopRequisition();
     IEnumerable<ShopRequisitionGodownsalesbill> getAllDetails();
     IEnumerable<ShopRequisitionGodownsalesbill> getListByPo(string value);
   //  IEnumerable<ShopRequisitionGodownitemsalesbill> GetDetailsByItemCodeAndFromShopNameIenumerable(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName);
    }
}
