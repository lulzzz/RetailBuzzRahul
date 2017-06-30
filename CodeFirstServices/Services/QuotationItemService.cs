using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class QuotationItemService : IQuotationItemService
    {
        private readonly IQuotationItemRepository _QuotationItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuotationItemService(IQuotationItemRepository QuotationItemRepository, IUnitOfWork unitOfWork)
        {
            this._QuotationItemRepository = QuotationItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<QuotationItem> GetDetailsByQuotNo(string quotno)
        {
            var list = _QuotationItemRepository.GetMany(l => l.QuotNo == quotno && l.Status == "Active");
            return list;
        }

        public void Create(QuotationItem QuotationItem)
        {
            _QuotationItemRepository.Add(QuotationItem);
            _unitOfWork.Commit();
        }

        public void Update(QuotationItem QuotationItem)
        {
            _QuotationItemRepository.Update(QuotationItem);
            _unitOfWork.Commit();
        }

        public IEnumerable<QuotationItem> GetItemsByCode(string code)
        {
            var details = _QuotationItemRepository.GetMany(q => q.QuotNo == code);
            return details;
        }

        public void Delete(QuotationItem QuotationItem)
        {
            _QuotationItemRepository.Delete(QuotationItem);
            _unitOfWork.Commit();
        }

        public QuotationItem GetRowByQuotNo(string quotno)
        {
            var data = _QuotationItemRepository.Get(m => m.QuotNo == quotno);
            return data;
        }

        public QuotationItem GetDetailsByItemCode(string itemcode)
        {
            var data = _QuotationItemRepository.Get(m => m.ItemCode == itemcode);
            return data;
        }

        public IEnumerable<QuotationItem> GetRowsByQuotNoAndStatus(string quotno)
        {
            var data = _QuotationItemRepository.GetMany(m => m.QuotNo == quotno && m.Status == "Active");
            return data;
        }

        public QuotationItem GetItemDetailsByItemCodeAndQuotNo(string itemcode, string quotno)
        {
            var data = _QuotationItemRepository.Get(m => m.ItemCode == itemcode && m.QuotNo == quotno);
            return data;
        }

        public IEnumerable<QuotationItem> GetInventoryItemsByQuotNo(string quotno)
        {
            var list = _QuotationItemRepository.GetMany(l => l.QuotNo == quotno && l.ItemType == "Inventory");
            return list;
        }

        public IEnumerable<QuotationItem> GetAllActiveItemsByQuotNo(string quotno)
        {
            var list = _QuotationItemRepository.GetMany(q => q.QuotNo == quotno && q.Status == "Active");
            return list;
        }
    }
}
