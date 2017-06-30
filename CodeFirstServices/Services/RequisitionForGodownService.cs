using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
 public  class RequisitionForGodownService:IRequisitionForGodownService
    {
      private readonly IRequisitionForGodownRepository _requisitionforgodownTypeRepository;
      private readonly IUnitOfWork _unitOfWork;

        public RequisitionForGodownService(IRequisitionForGodownRepository requisitionforgodownrepository, IUnitOfWork unitOfWork)
        {
            this._requisitionforgodownTypeRepository = requisitionforgodownrepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateRequisitionForGodown(RequisitionForGodown requisitionforgodown)
        {
            _requisitionforgodownTypeRepository.Add(requisitionforgodown);
            _unitOfWork.Commit();
        }

        public IEnumerable<RequisitionForGodown> GetRequisitionsForPo(string value)
        {
           var list=  _requisitionforgodownTypeRepository.GetMany(vl => vl.RequestToPo == value);
           return list;
        }

        public IEnumerable<RequisitionForGodown> getItemsIfRejectQuantityIsNotNull()
        {
            var list = _requisitionforgodownTypeRepository.GetMany(vl => vl.RejectQuantity != null);
            return list;
        }

        public IEnumerable<RequisitionForGodown> GetRowsByItemnameandGodownNameForRequisitionForGodown(string itemname, string godownname)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(d => d.ItemName == itemname && d.LoggedInGodownName == godownname);
            return data;
        }

        public IEnumerable<RequisitionForGodown> getItemDetailsByDateForRequisitionForGodown(DateTime fromdate, DateTime todate)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate);
            return data;
        }

        public IEnumerable<RequisitionForGodown> getItemDetailsByDateAndItemNameForRequisitionForGodown(DateTime fromdate, DateTime todate, string itemname)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate && c.ItemName == itemname);
            return data;
        }

        public IEnumerable<RequisitionForGodown> getDetailsOfToGodownIfNotNull()
        {
            var item = _requisitionforgodownTypeRepository.GetMany(c => c.ToGodownName != "null");
            return item;
        }

        public IEnumerable<RequisitionForGodown> getRequisitionForGodownDetailsiItemCodeAndDate(string code, DateTime? date, string godownname, string togodownname)
        {
            var item = _requisitionforgodownTypeRepository.GetMany(co => co.ItemCode == code && co.ModifiedOn == date && co.LoggedInGodownName == godownname && co.ToGodownName == togodownname);
            return item;

        }

        public RequisitionForGodown GetDetailsByIcAndfsAndDTAndRSForGodownUpdate(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownname)
        {
            var data = _requisitionforgodownTypeRepository.Get(d => d.ItemCode == itemcode && d.LoggedInGodownName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromgodownname);
            return data;
        }
        public IEnumerable<RequisitionForGodown> GetDetailsByICAndFShNameIenumerableForRequisitionOfgodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(d => d.ItemCode == itemcode && d.LoggedInGodownName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromshopName);
            return data;
        }

        public IEnumerable<RequisitionForGodown> GetDetailsByICodeAndSNIenumerableSkipFirstForGodownUpdate(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(d => d.ItemCode == itemcode && d.LoggedInGodownName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromshopName).Skip(1);
            return data;
        }

        public void Update(RequisitionForGodown requisitionforgodown)
        {
            _requisitionforgodownTypeRepository.Update(requisitionforgodown);
            _unitOfWork.Commit();
        }

        public IEnumerable<RequisitionForGodown> GetNamesByItemForRequisitionForGodown(string name, string shopname)
        {
            var namelist = _requisitionforgodownTypeRepository.GetMany(itemname => itemname.LoggedInGodownName == shopname && itemname.ItemName.ToLower().Contains(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return namelist;
        }

        public RequisitionForGodown GetLastRequisition()
        {
            var Details = _requisitionforgodownTypeRepository.GetAll().LastOrDefault();
            return Details;
        }

        public IEnumerable<RequisitionForGodown> GetDailyRequisitionsForGodowns(string loggedinshopname)
        {
            var list = _requisitionforgodownTypeRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == DateTime.Now.Date && s.ToGodownName != "null" && s.ToGodownName != null && s.LoggedInGodownName != loggedinshopname);
            return list;
        }

        public IEnumerable<RequisitionForGodown> GetDailyRequisitionsForSuperAdmin(string loggedinshopname)
        {
            var list = _requisitionforgodownTypeRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<RequisitionForGodown> getAllActiveRequisitions()
        {
            var item = _requisitionforgodownTypeRepository.GetMany(c => c.Balance != 0 && c.Balance != null);
            return item;
        }


        public IEnumerable<RequisitionForGodown> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date);
            return data;
        }

        public IEnumerable<RequisitionForGodown> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date && c.SubCategory == subcat);
            return data;
        }

        public IEnumerable<RequisitionForGodown> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode)
        {
            var data = _requisitionforgodownTypeRepository.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date && c.ItemCode == itemcode);
            return data;
        }

    }
}
