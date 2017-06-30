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
    public class SalesOrderItemService : ISalesOrderItemService
    {
        private readonly ISalesOrderItemRepository _salesorderitemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetAllItemsFromStkDistrbtnRepository _GetAllItemsFromStkDistrbtnRepository;
        private readonly IGetItemCodesByQuotAndItemCodeRepository _GetItemCodesByQuotAndItemCodeRepository;
        public SalesOrderItemService(ISalesOrderItemRepository salesorderitemRepository, IUnitOfWork unitOfWork, IGetAllItemsFromStkDistrbtnRepository GetAllItemsFromStkDistrbtnRepository, IGetItemCodesByQuotAndItemCodeRepository GetItemCodesByQuotAndItemCodeRepository)
        {
            this._salesorderitemRepository = salesorderitemRepository;
            this._unitOfWork = unitOfWork;
            this._GetAllItemsFromStkDistrbtnRepository = GetAllItemsFromStkDistrbtnRepository;
            this._GetItemCodesByQuotAndItemCodeRepository = GetItemCodesByQuotAndItemCodeRepository;
        }

        public void Create(SalesOrderItem data)
        {
            _salesorderitemRepository.Add(data);
            _unitOfWork.Commit();
        }

        public void Update(SalesOrderItem data)
        {
            _salesorderitemRepository.Update(data);
            _unitOfWork.Commit();
        }

        public void Delete(SalesOrderItem data)
        {
            _salesorderitemRepository.Delete(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<SalesOrderItem> GetDetailsByOrderNo(string orderno)
        {
            var data = _salesorderitemRepository.GetMany(d => d.OrderNo == orderno);
            return data;
        }

        public IEnumerable<GetAllItemsFromStkDistrbtn> GetItemCodeUsingStoredProc(string procname)
        {
            var data = _GetAllItemsFromStkDistrbtnRepository.ExecuteCustomStoredProc(procname);
            return data;
        }

        public SalesOrderItem GetDetailsById(int id)
        {
            var data = _salesorderitemRepository.GetById(id);
            return data;
        }

        public IEnumerable<SalesOrderItem> GetDetailsByQuotNo(string quotno)
        {
            var data = _salesorderitemRepository.GetMany(d => d.QuotationNo == quotno);
            return data;
        }

        public IEnumerable<SalesOrderItem> GetRowsByItemCode(string itemcode)
        {
            var details = _salesorderitemRepository.GetMany(d => d.ItemCode == itemcode);
            return details;
        }

        public IEnumerable<GetItemCodesByQuotAndItemCode> GetItemsByQuotItemCode(string procname, object[] id)
        {
            var data = _GetItemCodesByQuotAndItemCodeRepository.ExecuteCustomStoredProcByParam(procname, id);
            return data;
        }

        public SalesOrderItem GetDetailsByItemCodeAndQuot(string itemcode, string quotno)
        {
            var details = _salesorderitemRepository.Get(d => d.ItemCode == itemcode && d.QuotationNo == quotno);
            return details;
        }

        public IEnumerable<SalesOrderItem> GetRowsByItemCodeAndQuot(string itemcode, string quotno)
        {
            var details = _salesorderitemRepository.GetMany(d => d.ItemCode == itemcode && d.QuotationNo == quotno);
            return details;
        }

        public IEnumerable<SalesOrderItem> GetDetailsBySONoAndStatus(string orderno)
        {
            var data = _salesorderitemRepository.GetMany(d => d.OrderNo == orderno && d.Status == "Active");
            return data;
        }

        public SalesOrderItem GetItemDetailsByItemCodeandOrderNo(string itemcode, string orderno)
        {
            var data = _salesorderitemRepository.Get(m => m.ItemCode == itemcode && m.OrderNo == orderno);
            return data;
        }

        public IEnumerable<SalesOrderItem> GetDetailsBySONoandItemType(string orderno)
        {
            var data = _salesorderitemRepository.GetMany(d => d.OrderNo == orderno && d.ItemType == "Inventory");
            return data;
        }

        public IEnumerable<SalesOrderItem> GetAllActiveItemsByOrderNo(string orderno)
        {
            var list = _salesorderitemRepository.GetMany(s => s.OrderNo == orderno && s.Status == "Active");
            return list;
        }
    }
}
