using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IRetailBillAdjAmtDetailService
    {
        void Create(RetailBillAdjAmtDetail data);
        void Delete(RetailBillAdjAmtDetail data);
        IEnumerable<RetailBillAdjAmtDetail> GetBillsByDate(DateTime date);
        IEnumerable<RetailBillAdjAmtDetail> GetBillsByRetailNo(string no);
        RetailBillAdjAmtDetail GetDetailsBySalesOrderNo(string no);
    }
}
