using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;


namespace CodeFirstServices.Interfaces
{
    public interface ISalesBillAdjAmtDetailService
    {
        void Create(SalesBillAdjAmtDetail data);
        void Delete(SalesBillAdjAmtDetail data);
        IEnumerable<SalesBillAdjAmtDetail> GetBillsByDate(DateTime date);
        IEnumerable<SalesBillAdjAmtDetail> GetBillsBySalesNo(string no);
    }
}
