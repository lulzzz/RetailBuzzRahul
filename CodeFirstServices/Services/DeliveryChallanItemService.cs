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
    public class DeliveryChallanItemService : IDeliveryChallanItemService
    {
        private readonly IDeliveryChallanItemRepository _DeliveryChallanItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IGetItemsByQuotOrOrderNoRepository _GetItemsByQuotOrOrderNoRepository;

        public DeliveryChallanItemService(IDeliveryChallanItemRepository DeliveryChallanItemRepository, IUnitOfWork UnitOfWork, IGetItemsByQuotOrOrderNoRepository GetItemsByQuotOrOrderNoRepository)
        {
            this._DeliveryChallanItemRepository = DeliveryChallanItemRepository;
            this._UnitOfWork = UnitOfWork;
            this._GetItemsByQuotOrOrderNoRepository = GetItemsByQuotOrOrderNoRepository;
        }

        public void Create(DeliveryChallanItem DeliveryChallanItem)
        {
            _DeliveryChallanItemRepository.Add(DeliveryChallanItem);
            _UnitOfWork.Commit();
        }

        public void Update(DeliveryChallanItem DeliveryChallanItem)
        {
            _DeliveryChallanItemRepository.Update(DeliveryChallanItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<DeliveryChallanItem> GetDetailsByChallanNo(string no)
        {
            var rows = _DeliveryChallanItemRepository.GetMany(n => n.ChallanNo == no);
            return rows;
        }

        public double GetAllQuantityByItemCode(string itemcode)
        {
            double quantity = _DeliveryChallanItemRepository.GetMany(m => m.ItemCode == itemcode && m.Status == "Active").Sum(m => Convert.ToDouble(m.Quantity));
            return quantity;
        }


        public IEnumerable<DeliveryChallanItem> GetDetailsByQuotNo(string QuotNo)
        {
            var data = _DeliveryChallanItemRepository.GetMany(q => q.QuotOrOrderNo == QuotNo);
            return data;
        }

        public DeliveryChallanItem GetDetailsByItemCodeAndQuot(string itemcode, string quotno)
        {
            var details = _DeliveryChallanItemRepository.Get(d => d.ItemCode == itemcode && d.QuotOrOrderNo == quotno);
            return details;
        }

        public IEnumerable<DeliveryChallanItem> GetRowsByItemCodeAndQuot(string itemcode, string quotno)
        {
            var details = _DeliveryChallanItemRepository.GetMany(d => d.ItemCode == itemcode && d.QuotOrOrderNo == quotno);
            return details;
        }

        public DeliveryChallanItem GetDetailsByQuotation(string quotno)
        {
            var data = _DeliveryChallanItemRepository.Get(m => m.QuotOrOrderNo == quotno);
            return data;
        }

        public IEnumerable<GetItemsByQuotOrOrderNo> GetItemsByQuotOrOrderNo(string procname, object[] id)
        {
            var data = _GetItemsByQuotOrOrderNoRepository.ExecuteCustomStoredProcByParam(procname, id);
            return data;
        }

        public DeliveryChallanItem GetItemDetailsByItemCodeAndQuotOrOrderNo(string itemcode, string quotno)
        {
            var data = _DeliveryChallanItemRepository.Get(m => m.ItemCode == itemcode && m.QuotOrOrderNo == quotno);
            return data;
        }

        public IEnumerable<DeliveryChallanItem> GetDetailsByChallanNoAndStatus(string no)
        {
            var rows = _DeliveryChallanItemRepository.GetMany(n => n.ChallanNo == no && n.Status == "Active"); ;
            return rows;
        }

        public DeliveryChallanItem GetDetailsByItemCodeAndChallanNo(string itemcode, string challanno)
        {
            var data = _DeliveryChallanItemRepository.Get(m => m.ItemCode == itemcode && m.ChallanNo == challanno);
            return data;
        }


        public void Delete(DeliveryChallanItem DeliveryChallanItem)
        {
            _DeliveryChallanItemRepository.Delete(DeliveryChallanItem);
            _UnitOfWork.Commit();
        }

        public DeliveryChallanItem GetById(int id)
        {
            var data = _DeliveryChallanItemRepository.GetById(id);
            return data;
        }

        public IEnumerable<DeliveryChallanItem> GetInventoryItemsByChallanNo(string no)
        {
            var rows = _DeliveryChallanItemRepository.GetMany(n => n.ChallanNo == no && n.ItemType == "Inventory");
            return rows;
        }

        public IEnumerable<DeliveryChallanItem> GetAllActiveItemsByChallanNo(string challanno)
        {
            var details = _DeliveryChallanItemRepository.GetMany(dc => dc.ChallanNo == challanno && dc.Status == "Active");
            return details;
        }
    }
}
