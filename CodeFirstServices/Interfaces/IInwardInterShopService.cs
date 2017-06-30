using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardInterShopService
    {
        InwardInterShop GetLastInward();
        void Create(InwardInterShop inward);
        InwardInterShop GetDetailsById(int Id);
        IEnumerable<InwardInterShop> GetInwardNo(string term, string ShopCode);
        InwardInterShop GetDetailsByInwardNo(string InwardNo);
        IEnumerable<InwardInterShop> GetDetailsByDate(DateTime FromDate, DateTime ToDate);
        IEnumerable<InwardInterShop> GetDailyInwardInterShop();
        InwardInterShop GetLastInwardByFinYear(string Year,string ShopCode);
    }
}
