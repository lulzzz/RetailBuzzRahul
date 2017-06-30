using CodeFirstEntities;
using CodeFirstServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardShopToGodownService
    {
        OutwardShopToGodown GetLastRow();
        void Create(OutwardShopToGodown outward);
        void Update(OutwardShopToGodown outward);
        OutwardShopToGodown GetDetailsById(int Id);
        OutwardShopToGodown GetDetailsByOutwardNo(string OutwardNo);
        IEnumerable<OutwardShopToGodown> GetOutwardNos(string term,string GodownCode);
        IEnumerable<OutwardShopToGodown> GetDetailsByDate(DateTime FromDate, DateTime ToDate);
        IEnumerable<OutwardShopToGodown> GetDetailsByShopAndDate(string Shop, DateTime FromDate, DateTime ToDate);
        IEnumerable<OutwardShopToGodown> GetOutwardNoForPrint(string term, string ShopCode);
        IEnumerable<OutwardShopToGodown> GetDailyOutwardsToGodown();
        OutwardShopToGodown GetLastOutwardByFinYear(string Year,string ShopCode);
        void Delete(OutwardShopToGodown outward);
    }
}
