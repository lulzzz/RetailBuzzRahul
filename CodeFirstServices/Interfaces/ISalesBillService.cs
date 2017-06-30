using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;
namespace CodeFirstServices.Interfaces
{
    public interface ISalesBillService
    {
        void Create(SalesBill bill);
        void Update(SalesBill bill);
        SalesBill GetLastSalesBillByFinYr(string year, string shopcode);
        SalesBill GetDetailsById(int id);
        IEnumerable<SalesBill> GetAll();
        IEnumerable<SalesBill> GetSalesBillNos(string billno);
        SalesBill GetDetailsBySalesBillNo(string no);
        IEnumerable<SalesBill> GetSalesBillByDate();
        IEnumerable<SalesBill> GetSalesBillByClient(string client);
        IEnumerable<SalesBill> GetSalesBillNosByCashierStatus(string billno);
        IEnumerable<SalesBill> GetBillsByDateAndAdjustAmount(DateTime date);
        IEnumerable<SalesBill> GetBillsByClientName(string client);
        IEnumerable<SalesBill> GetSalesBillNosByWithTaxStatus(string term);
        IEnumerable<SalesBill> GetSalesBillNosByWithoutTaxStatus(string term);
        IEnumerable<SalesBill> GetSalesBillByFromAndToDate(DateTime fromdate, DateTime todate);
        IEnumerable<SalesBill> GetAllActiveSalesBill();
        IEnumerable<SalesBill> GetAllSalesBill(string term);
        IEnumerable<SalesBill> GetBillsByDate(DateTime date);
    }
}
