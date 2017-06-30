using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeFirstServices.Interfaces
{
 public interface IOutwardInterShopService
    {
     OutwardInterShop GetLastRow();
     void CreateOutwardInterShop(OutwardInterShop outwardintershop);
     OutwardInterShop GetDetailsByOutwardId(int id);
     OutwardInterShop GetDetailsByOutwardCode(string OutwardCode);
     IEnumerable<OutwardInterShop> GetOutwardNo(string term,string ShopCode);
     IEnumerable<OutwardInterShop> GetOutwardNoForPrint(string term, string ShopCode);
     void Update(OutwardInterShop outward);
     IEnumerable<OutwardInterShop> GetDetailsByDate(DateTime FromDate, DateTime ToDate);
     IEnumerable<OutwardInterShop> GetDailyOutwardInterShop();
     OutwardInterShop GetLastOutwardByFinYear(string Year,string ShopCode);
     void Delete(OutwardInterShop outward);
    }
}
