using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class CashierRefundOrderService:ICashierRefundOrderService
    {
        private readonly ICashierRefundOrderRepository _refundsalesorderrepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierRefundOrderService(ICashierRefundOrderRepository refundsalesorderrepository, IUnitOfWork unitOfWork)
        {
            this._refundsalesorderrepository = refundsalesorderrepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierRefundOrder data)
        {
            _refundsalesorderrepository.Add(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierRefundOrder> GetBillsByDate(DateTime date)
        {
            var data = _refundsalesorderrepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public IEnumerable<CashierRefundOrder> GetAll()
        {
            var data = _refundsalesorderrepository.GetAll();
            return data;
        }

        public IEnumerable<CashierRefundOrder> GetDailyCashierRefundOrder()
        {
            var list = _refundsalesorderrepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }
    }
}
