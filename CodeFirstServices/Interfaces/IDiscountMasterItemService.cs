using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IDiscountMasterItemService
    {
        void Create(DiscountMasterItem discount);
        DiscountMasterItem getLastRow();
        DiscountMasterItem GetLastRowrByFinYr(string year);
        DiscountMasterItem GetDetailsById(int id);
        IEnumerable<DiscountMasterItem> GetRowsByCode(string code);
        DiscountMasterItem GetDetailsByItemCodeAndFromDate(string itemcode, DateTime fromdate);
        DiscountMasterItem GetDailyDiscountByItemCode(string itemcode);
    }
}
