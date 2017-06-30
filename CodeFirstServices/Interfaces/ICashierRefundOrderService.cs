using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierRefundOrderService
    {
        void Create(CashierRefundOrder data);
        IEnumerable<CashierRefundOrder> GetBillsByDate(DateTime date);
        IEnumerable<CashierRefundOrder> GetAll();
        IEnumerable<CashierRefundOrder> GetDailyCashierRefundOrder();
    }
}

