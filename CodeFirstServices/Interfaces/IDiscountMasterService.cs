using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IDiscountMasterService
    {
        void Create(DiscountMaster discount);
        DiscountMaster getLastRow();
        DiscountMaster GetLastRowrByFinYr(string year);
        DiscountMaster GetDetailsById(int id);
        IEnumerable<DiscountMaster> getItemsByDiscount(DateTime? year);
    }
}
