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
    public class CashierSalesOrderItemService:ICashierSalesOrderItemService
    {
        private readonly ICashierSalesOrderItemRepository _CashierSalesOrderItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierSalesOrderItemService(ICashierSalesOrderItemRepository CashierSalesOrderItemRepository, IUnitOfWork unitOfWork)
        {
            this._CashierSalesOrderItemRepository = CashierSalesOrderItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierSalesOrderItem cash)
        {
            _CashierSalesOrderItemRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierSalesOrderItem> GetDetailsByCashierCode(string code)
        {
            var data = _CashierSalesOrderItemRepository.GetMany(d => d.CashierCode == code);
            return data;
        }

        public IEnumerable<CashierSalesOrderItem> GetRowsBySONo(string orderno)
        {
            var data = _CashierSalesOrderItemRepository.GetMany(d => d.OrderNo == orderno);
            return data;
        }

    }
}
