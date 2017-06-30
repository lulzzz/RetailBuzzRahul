using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IOutwardToShopService
    {
        void Create(OutwardToShop godown);
        void Udpate(OutwardToShop godown);
        OutwardToShop GetLastRow();
        OutwardToShop GetLastOutwardByFinYr(string year,string GdCode);
        IEnumerable<OutwardToShop> GetDetailsByDate(DateTime fromdate, DateTime todate);
        IEnumerable<OutwardToShop> GetDetailsByGodownCode(string godowncode);
        OutwardToShop GetDetailsbyId(int id);
        IEnumerable<OutwardToShop> GetActiveOutwards();
        OutwardToShop GetDetailsByOutwardCode(string outwardcode);
        IEnumerable<OutwardToShop> GetOutwardNo(string term,string shopcode);
        IEnumerable<OutwardToShop> GetOutwardNoForPrint(string term, string godowncode);
        IEnumerable<OutwardToShop> GetDailyOutwardsToShop();
        void Delete(OutwardToShop outward);
    }
}
