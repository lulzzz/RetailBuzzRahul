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
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _SalesOrderRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public SalesOrderService(ISalesOrderRepository _SalesOrderRepository, IUnitOfWork unitOfWork)
        {
            this._SalesOrderRepository = _SalesOrderRepository;
            this._UnitOfWork = unitOfWork;
        }

        public void Create(SalesOrder data)
        {
            _SalesOrderRepository.Add(data);
            _UnitOfWork.Commit();
        }

        public SalesOrder GetLastRow()
        {
            var row = _SalesOrderRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<SalesOrder> GetOrderNos(string order)
        {
            var data = _SalesOrderRepository.GetMany(b => b.OrderNo.ToLower().Contains(order.ToString().ToLower()) && b.Status == "Active" && b.SendingStatus == "Pending" && b.Editable == "Yes" && b.CashierStatus == "Active").OrderBy(b => b.OrderNo);
            return data;
        }

        public SalesOrder GetOrderDetails(string code)
        {
            var data = _SalesOrderRepository.Get(s => s.QuotationNo == code);
            return data;
        }

        public SalesOrder GetDetailsBySalesOrderNo(string no)
        {
            var data = _SalesOrderRepository.Get(n => n.OrderNo == no);
            return data;
        }

        public void Update(SalesOrder OrderProcessing)
        {
            _SalesOrderRepository.Update(OrderProcessing);
            _UnitOfWork.Commit();
        }

        public IEnumerable<SalesOrder> GetOrdersByStatus(string orderno)
        {
            var data = _SalesOrderRepository.GetMany(b => b.Status == "Active" && b.CashierStatus == "Active" && b.OrderNo.ToLower().Contains(orderno.ToString().ToLower())).OrderBy(b => b.OrderNo);
            return data;
        }

        public IEnumerable<SalesOrder> GetAll()
        {
            var data = _SalesOrderRepository.GetAll();
            return data;
        }

        public SalesOrder GetDetailsById(int id)
        {
            var details = _SalesOrderRepository.GetById(id);
            return details;
        }

        public IEnumerable<SalesOrder> GetAllActiveSalesOrder()
        {
            var details = _SalesOrderRepository.GetMany(m => m.Status == "Active");
            return details;
        }

        public SalesOrder GetDetailsByOrderNo(string orderno)
        {
            var details = _SalesOrderRepository.Get(d => d.OrderNo == orderno && d.Status == "Active");
            return details;
        }

        public IEnumerable<SalesOrder> GetDetailsByQuotNo(string quotno)
        {
            var details = _SalesOrderRepository.GetMany(d => d.QuotationNo == quotno);
            return details;
        }


        public IEnumerable<SalesOrder> GetDetailsByStatusAndCashierStatus(string term)
        {
            var details = _SalesOrderRepository.GetMany(d => d.Status == "Active" && d.CashierStatus == "InActive" && d.OrderNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(d => d.OrderNo);
            return details;
        }

        public IEnumerable<SalesOrder> GetDetailsByClientName(string client)
        {
            var details = _SalesOrderRepository.GetMany(d => d.ClientName == client && d.Status == "Active" && d.CashierStatus == "InActive");
            return details;
        }

        public IEnumerable<SalesOrder> GetDetailsByClientAndBalance(string client)
        {
            var details = _SalesOrderRepository.GetMany(d => d.ClientName == client && d.Status == "Active" && d.CashierStatus == "InActive" && d.RemainingAdvance != 0);
            return details;
        }

        public IEnumerable<SalesOrder> GetActiveSalesOrderForAutoComplete(string orderno)
        {
            var data = _SalesOrderRepository.GetMany(b => b.Status == "Active" && b.OrderNo.ToLower().Contains(orderno.ToString().ToLower())).OrderBy(b => b.OrderNo);
            return data;
        }

        public IEnumerable<SalesOrder> GetDailySalesOrders()
        {
            var list = _SalesOrderRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
            return list;
        }

        public SalesOrder GetLastSalerOrderByFinYr(string year, string shopcode)
        {
            var data = _SalesOrderRepository.GetMany(s => s.OrderNo.Contains(year) && s.ShopCode == shopcode).OrderBy(s => s.OrderNo).LastOrDefault();
            return data;
        }

        public IEnumerable<SalesOrder> GetAllSalesOrders(string term)
        {
            var list = _SalesOrderRepository.GetMany(b => b.OrderNo.ToLower().Contains(term.ToString().ToLower())).OrderBy(b => b.OrderNo);
            return list;
        }
    }
}
