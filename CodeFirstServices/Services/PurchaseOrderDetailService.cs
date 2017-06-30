using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFirstServices.Interfaces;
using System.Threading.Tasks;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class PurchaseOrderDetailService : IPurchaseOrderDetailService
    {
        private readonly IPurchaseOrderDetailRepository _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderDetailService(IPurchaseOrderDetailRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
        {
            this._purchaseOrderRepository = purchaseOrderRepository;
            this._unitOfWork = unitOfWork;
        }
        public void CreatePurchaseOrder(PurchaseOrderDetail PurchaseOrderDetail)
        {
            _purchaseOrderRepository.Add(PurchaseOrderDetail);
            _unitOfWork.Commit();
        }

        public IEnumerable<PurchaseOrderDetail> GetAllPurchaseOrder()
        {
            var PurchaseOrderDetail = _purchaseOrderRepository.GetAll();
            return PurchaseOrderDetail;
        }

        public PurchaseOrderDetail GetPurchaseOrderbyId(int id)
        {
            var PurchaseOrderDetail = _purchaseOrderRepository.Get(type => type.PoId == id);
            return PurchaseOrderDetail;
        }

        public IEnumerable<PurchaseOrderDetail> GetPendingPoNosBySupplier(string suppliername)
        {
            var polist = _purchaseOrderRepository.GetMany(po => po.SupplierName == suppliername && po.ReceivingStatus != "Completed");
            return polist;
        }

        public PurchaseOrderDetail GetDetailByPoNo(string pono)
        {
            var podetails = _purchaseOrderRepository.Get(po => po.PoNo == pono);
            return podetails;
        }

        public IEnumerable<PurchaseOrderDetail> getReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _purchaseOrderRepository.GetMany(cl => cl.PoDate.Date >= fromdate.Date && cl.PoDate.Date <= todate.Date);
            return value;
        }

        public IEnumerable<PurchaseOrderDetail> GetReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate)
        {
            var data = _purchaseOrderRepository.GetMany(d => d.SupplierName == Supplier && d.modifiedOn.Date >= fromdate.Date && d.modifiedOn.Date <= todate.Date);
            return data;
        }

        public IEnumerable<PurchaseOrderDetail> GetPOesPendingsList(DateTime fromdate, DateTime todate)
        {
            var data = _purchaseOrderRepository.GetMany(p => p.PoDate.Date >= fromdate.Date && p.PoDate.Date <= todate.Date && p.status == "Active" && p.ReceivingStatus != "Completed");
            return data;
        }

        public void UpdatePurchaseOrder(PurchaseOrderDetail PurchaseOrderDetail)
        {
            _purchaseOrderRepository.Update(PurchaseOrderDetail);
            _unitOfWork.Commit();
        }

        public IEnumerable<PurchaseOrderDetail> GetPendingPo(string receivingstatus)
        {
            var data = _purchaseOrderRepository.GetMany(p => p.ReceivingStatus != "Completed");
            return data;
        }

        public IEnumerable<PurchaseOrderDetail> GetInwardDetailsByDate()
        {
            var data = _purchaseOrderRepository.GetMany(p => Convert.ToDateTime(p.DelDate).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<PurchaseOrderDetail> GetPOList(string term)
        {
            var data = _purchaseOrderRepository.GetMany(p => p.PoNo.ToLower().Contains(term.ToString().ToLower()) && p.ReceivingStatus == "Pending");
            return data;
        }

        public PurchaseOrderDetail GetLastPOByFinYr(string year, string godownshopcode)
        {
            if (godownshopcode.Contains("SH"))
            {
                var data = _purchaseOrderRepository.GetMany(p => p.PoNo.Contains(year) && p.ShopCode == godownshopcode).OrderBy(p => p.PoNo).LastOrDefault();
                return data;
            }
            else
            {
                var data = _purchaseOrderRepository.GetMany(p => p.PoNo.Contains(year) && p.GodownCode == godownshopcode).OrderBy(p => p.PoNo).LastOrDefault();
                return data;
            }
        }

        public IEnumerable<PurchaseOrderDetail> GetPendingPoForCarryForward()
        {
            var data = _purchaseOrderRepository.GetMany(p => p.ReceivingStatus != "Completed" && p.status == "Active");
            return data;
        }

        public IEnumerable<PurchaseOrderDetail> GetAllPOesForPrint(string term)
        {
            var list = _purchaseOrderRepository.GetMany(p => p.PoNo.ToLower().Contains(term.ToString().ToLower()));
            return list;
        }
    }
}

