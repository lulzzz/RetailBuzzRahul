using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IPurchaseItemTaxService
    {
        void Create(PurchaseItemTaxMaster tax);
        IEnumerable<PurchaseItemTaxMaster> GetAll();
        IEnumerable<PurchaseItemTaxMaster> GetDetailsByDate(DateTime fromdate, DateTime todate, string taxtype);
        PurchaseItemTaxMaster GetLastRow();
        IEnumerable<PurchaseItemTaxMaster> GetInsertedRows(int lastrowbefore, int lastrowafter);
        PurchaseItemTaxMaster GetLatestTax(string type, string subcat);
        PurchaseItemTaxMaster GetLatestTaxByState(string type, string subcat, string state);
        PurchaseItemTaxMaster GetTaxBySubCatAndVAT(string subcat);
    }
}
