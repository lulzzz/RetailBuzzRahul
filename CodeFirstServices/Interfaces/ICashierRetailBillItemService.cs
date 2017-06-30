using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierRetailBillItemService
    {
        void Create(CashierRetailBillItem cash);
        IEnumerable<CashierRetailBillItem> GetDetailsByCashierCode(string code);
        IEnumerable<CashierRetailBillItem> GetRowsByRetailBillNo(string retailno);
    }
}
