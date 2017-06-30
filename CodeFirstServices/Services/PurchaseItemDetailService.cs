using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class PurchaseItemDetailService : IPurchaseItemDetailService
    {
        private readonly IPurchaseItemDetailRepository _purchaseItemDetail;
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseItemDetailService(IPurchaseItemDetailRepository purchaseItemDetail, IUnitOfWork  unitOfWork)
        {
            this._purchaseItemDetail = purchaseItemDetail;
            this._unitOfWork = unitOfWork;
        }
        public void CreatePurchaseItemDetail(PurchaseItemDetail PurchaseDetail)
        {
            _purchaseItemDetail.Add(PurchaseDetail);
            _unitOfWork.Commit();
        }

        public IEnumerable<PurchaseItemDetail> GetPurchaseItemsByPONo(string PoNo)
        {
            var details = _purchaseItemDetail.GetMany(po => po.PoNo == PoNo);
            return details;
        }
        public IEnumerable<PurchaseItemDetail> GetPurchaseInventoryItemsByPONo(string PoNo)
        {
            var details = _purchaseItemDetail.GetMany(po => po.PoNo == PoNo && po.ItemType == "Inventory");
            return details;
        }

        public void UpdatePurchaseItemDetail(PurchaseItemDetail PurchaseDetail)
        {
            _purchaseItemDetail.Update(PurchaseDetail);
            _unitOfWork.Commit();
        }

        public void DeletePurchaseItemDetail(PurchaseItemDetail PurchaseDetail)
        {
            _purchaseItemDetail.Delete(PurchaseDetail);
            _unitOfWork.Commit();
        }

        public PurchaseItemDetail GetDetailsById(int id)
        {
            var data = _purchaseItemDetail.GetById(id);
            return data;
        }

        public IEnumerable<PurchaseItemDetail> GetReportByItemAndDate(string Item, DateTime fromdate, DateTime todate)
        {
            var data = _purchaseItemDetail.GetMany(d => d.itemCode == Item && d.ModifiedOn.Date >= fromdate.Date && d.ModifiedOn.Date <= todate.Date); 
            return data;
        }
    }
}
