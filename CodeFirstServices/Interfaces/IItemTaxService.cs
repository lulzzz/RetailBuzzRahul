using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeFirstServices.Interfaces
{
    public interface IItemTaxService
    {
        void Create(ItemTaxMaster tax);
        IEnumerable<ItemTaxMaster> GetAll();
        IEnumerable<ItemTaxMaster> GetDetailsByDate(DateTime fromdate, DateTime todate, string taxtype);
        ItemTaxMaster GetLastRow();
        IEnumerable<ItemTaxMaster> GetInsertedRows(int lastrowbefore, int lastrowafter);
        ItemTaxMaster GetLatestTax(string type,string subcat);
        ItemTaxMaster GetTaxBySubCatAndVAT(string subcat);
    }
}
